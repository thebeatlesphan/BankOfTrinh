using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Ledger;

public sealed record PostingLine(Guid AccountId, long AmountCents, EntryDirection Direction);

public sealed record PostingRequest(
    string IdempotencyKey,
    string Description,
    string SourceType,
    string SourceId,
    IReadOnlyList<PostingLine> Lines);

public interface IPostingService
{
    /// <summary>
    /// Posts a balanced transaction. Safe to retry: the same key with the same content returns the
    /// original transaction; the same key with different content throws <see cref="IdempotencyConflictException"/>.
    /// </summary>
    Task<LedgerTransaction> PostAsync(PostingRequest request, CancellationToken ct = default);

    /// <summary>Posts a mirror-image transaction that cancels <paramref name="transactionId"/>. A transaction can be reversed once.</summary>
    Task<LedgerTransaction> ReverseAsync(Guid transactionId, string idempotencyKey, string reason, CancellationToken ct = default);

    /// <summary>Balance in the account's normal direction (assets/expenses: debits minus credits; liabilities/revenue: credits minus debits).</summary>
    Task<long> GetBalanceAsync(Guid accountId, CancellationToken ct = default);

    /// <summary>Total debits and credits across the whole ledger. They must always be equal.</summary>
    Task<(long DebitsCents, long CreditsCents)> GetTrialBalanceAsync(CancellationToken ct = default);
}

public sealed class PostingService(LedgerDbContext db, TimeProvider clock) : IPostingService
{
    private const int MaxKeyLength = 200;

    public Task<LedgerTransaction> PostAsync(PostingRequest request, CancellationToken ct = default)
        => CreateAsync(request, reverses: null, ct);

    public async Task<LedgerTransaction> ReverseAsync(
        Guid transactionId, string idempotencyKey, string reason, CancellationToken ct = default)
    {
        var original = await db.Transactions
            .Include(t => t.Entries)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == transactionId, ct)
            ?? throw new LedgerException($"Transaction {transactionId} not found.");

        if (original.ReversesTransactionId is not null)
            throw new LedgerException("A reversal cannot itself be reversed. Post a new transaction instead.");

        var request = new PostingRequest(
            idempotencyKey,
            $"Reversal of {original.Id}: {reason}",
            original.SourceType,
            original.SourceId,
            original.Entries
                .Select(e => new PostingLine(
                    e.AccountId,
                    e.AmountCents,
                    e.Direction == EntryDirection.Debit ? EntryDirection.Credit : EntryDirection.Debit))
                .ToList());

        try
        {
            return await CreateAsync(request, reverses: original.Id, ct);
        }
        catch (DbUpdateException)
        {
            // The unique index on ReversesTransactionId rejects a second reversal sent with a different key.
            if (await AlreadyReversedAsync(original.Id, ct))
                throw new LedgerException($"Transaction {original.Id} has already been reversed.");
            throw;
        }
    }

    public async Task<long> GetBalanceAsync(Guid accountId, CancellationToken ct = default)
    {
        var account = await db.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == accountId, ct)
            ?? throw new LedgerException($"Account {accountId} not found.");

        var debitMinusCredit = await db.Entries
            .Where(e => e.AccountId == accountId)
            .SumAsync(e => e.Direction == EntryDirection.Debit ? e.AmountCents : -e.AmountCents, ct);

        return account.Type is AccountType.Asset or AccountType.Expense ? debitMinusCredit : -debitMinusCredit;
    }

    public async Task<(long DebitsCents, long CreditsCents)> GetTrialBalanceAsync(CancellationToken ct = default)
    {
        var debits = await db.Entries
            .Where(e => e.Direction == EntryDirection.Debit)
            .SumAsync(e => e.AmountCents, ct);
        var credits = await db.Entries
            .Where(e => e.Direction == EntryDirection.Credit)
            .SumAsync(e => e.AmountCents, ct);
        return (debits, credits);
    }

    private async Task<LedgerTransaction> CreateAsync(PostingRequest request, Guid? reverses, CancellationToken ct)
    {
        Validate(request);

        var existing = await FindByKeyAsync(request.IdempotencyKey, ct);
        if (existing is not null)
            return EnsureSameRequest(existing, request, reverses);

        var accountIds = request.Lines.Select(l => l.AccountId).Distinct().ToList();
        var accounts = await db.Accounts.AsNoTracking()
            .Where(a => accountIds.Contains(a.Id))
            .ToListAsync(ct);

        if (accounts.Count != accountIds.Count)
            throw new LedgerException("One or more accounts in the posting do not exist.");
        if (accounts.Select(a => a.Currency).Distinct().Count() > 1)
            throw new LedgerException("All accounts in a posting must share the same currency.");

        var tx = new LedgerTransaction
        {
            Id = Guid.NewGuid(),
            IdempotencyKey = request.IdempotencyKey,
            Description = request.Description,
            SourceType = request.SourceType,
            SourceId = request.SourceId,
            ReversesTransactionId = reverses,
            CreatedUtc = clock.GetUtcNow().UtcDateTime,
            Entries = request.Lines.Select(l => new LedgerEntry
            {
                Id = Guid.NewGuid(),
                AccountId = l.AccountId,
                AmountCents = l.AmountCents,
                Direction = l.Direction,
            }).ToList(),
        };

        db.Transactions.Add(tx);
        try
        {
            // One SaveChanges is one database transaction: the header and all entries land together or not at all.
            await db.SaveChangesAsync(ct);
            return tx;
        }
        catch (DbUpdateException)
        {
            // Another request may have won a race on the same idempotency key.
            // Clear() drops this context's tracked state, so use a scoped DbContext per request.
            db.ChangeTracker.Clear();
            var winner = await FindByKeyAsync(request.IdempotencyKey, ct);
            if (winner is null) throw;
            return EnsureSameRequest(winner, request, reverses);
        }
    }

    private static void Validate(PostingRequest r)
    {
        if (string.IsNullOrWhiteSpace(r.IdempotencyKey) || r.IdempotencyKey.Length > MaxKeyLength)
            throw new UnbalancedTransactionException($"IdempotencyKey is required and at most {MaxKeyLength} characters.");
        if (string.IsNullOrWhiteSpace(r.Description))
            throw new UnbalancedTransactionException("Description is required.");
        if (string.IsNullOrWhiteSpace(r.SourceType) || string.IsNullOrWhiteSpace(r.SourceId))
            throw new UnbalancedTransactionException("SourceType and SourceId are required: every entry must point to its source document.");
        if (r.Lines is null || r.Lines.Count < 2)
            throw new UnbalancedTransactionException("A posting needs at least two lines.");
        if (r.Lines.Any(l => l.AmountCents <= 0))
            throw new UnbalancedTransactionException("Line amounts must be positive whole cents.");
        if (r.Lines.Any(l => !Enum.IsDefined(l.Direction)))
            throw new UnbalancedTransactionException("Line direction must be Debit or Credit.");

        long debits, credits;
        try
        {
            debits = r.Lines.Where(l => l.Direction == EntryDirection.Debit).Sum(l => checked(l.AmountCents));
            credits = r.Lines.Where(l => l.Direction == EntryDirection.Credit).Sum(l => checked(l.AmountCents));
        }
        catch (OverflowException)
        {
            throw new UnbalancedTransactionException("Posting totals are too large.");
        }

        if (debits != credits)
            throw new UnbalancedTransactionException(
                $"Debits ({debits}) and credits ({credits}) must be equal.");
    }

    private Task<LedgerTransaction?> FindByKeyAsync(string key, CancellationToken ct)
        => db.Transactions.AsNoTracking()
            .Include(t => t.Entries)
            .FirstOrDefaultAsync(t => t.IdempotencyKey == key, ct);

    private Task<bool> AlreadyReversedAsync(Guid originalId, CancellationToken ct)
        => db.Transactions.AsNoTracking().AnyAsync(t => t.ReversesTransactionId == originalId, ct);

    private static LedgerTransaction EnsureSameRequest(LedgerTransaction existing, PostingRequest request, Guid? reverses)
    {
        static List<(Guid, long, EntryDirection)> Normalize(IEnumerable<(Guid, long, EntryDirection)> lines)
            => lines.OrderBy(l => l.Item1).ThenBy(l => l.Item3).ThenBy(l => l.Item2).ToList();

        var same =
            existing.SourceType == request.SourceType
            && existing.SourceId == request.SourceId
            && existing.ReversesTransactionId == reverses
            && Normalize(existing.Entries.Select(e => (e.AccountId, e.AmountCents, e.Direction)))
                .SequenceEqual(Normalize(request.Lines.Select(l => (l.AccountId, l.AmountCents, l.Direction))));

        return same ? existing : throw new IdempotencyConflictException(request.IdempotencyKey);
    }
}
