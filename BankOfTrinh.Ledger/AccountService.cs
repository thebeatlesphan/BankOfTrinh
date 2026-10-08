using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Ledger;

public interface IAccountService
{
    /// <summary>Returns the account with this code, creating it if needed. An existing account must match the requested type.</summary>
    Task<Account> GetOrCreateAsync(
        string code, string name, AccountType type,
        string? ownerType = null, string? ownerId = null,
        CancellationToken ct = default);
}

public sealed class AccountService(LedgerDbContext db, TimeProvider clock) : IAccountService
{
    public async Task<Account> GetOrCreateAsync(
        string code, string name, AccountType type,
        string? ownerType = null, string? ownerId = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new LedgerException("Account code is required.");

        var existing = await db.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Code == code, ct);
        if (existing is not null) return Check(existing, type);

        var account = new Account
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            Type = type,
            OwnerType = ownerType,
            OwnerId = ownerId,
            Currency = "USD",
            CreatedUtc = clock.GetUtcNow().UtcDateTime,
        };

        db.Accounts.Add(account);
        try
        {
            await db.SaveChangesAsync(ct);
            return account;
        }
        catch (DbUpdateException)
        {
            // Lost a race on the unique Code index; use the winner.
            db.Entry(account).State = EntityState.Detached;
            var winner = await db.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Code == code, ct);
            if (winner is null) throw;
            return Check(winner, type);
        }
    }

    private static Account Check(Account account, AccountType requested)
        => account.Type == requested
            ? account
            : throw new LedgerException($"Account '{account.Code}' exists as {account.Type}, not {requested}.");
}
