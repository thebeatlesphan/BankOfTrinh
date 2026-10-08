using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Ledger.Tests;

/// <summary>
/// Runs against in-memory SQLite, which (unlike the EF InMemory provider) enforces unique indexes,
/// foreign keys, check constraints and real transactions.
/// </summary>
public sealed class PostingServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly LedgerDbContext _db;
    private readonly PostingService _posting;
    private readonly AccountService _accounts;

    private Account _clearing = null!;   // Asset
    private Account _merchant = null!;   // Liability
    private Account _fees = null!;       // Revenue

    public PostingServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _connection.CreateCommand().Also(c => { c.CommandText = "PRAGMA foreign_keys = ON;"; c.ExecuteNonQuery(); });

        var options = new DbContextOptionsBuilder<LedgerDbContext>().UseSqlite(_connection).Options;
        _db = new LedgerDbContext(options);
        _db.Database.EnsureCreated();

        _posting = new PostingService(_db, TimeProvider.System);
        _accounts = new AccountService(_db, TimeProvider.System);

        _clearing = _accounts.GetOrCreateAsync("SYS:SETTLEMENT_CLEARING", "Settlement clearing", AccountType.Asset).Result;
        _merchant = _accounts.GetOrCreateAsync("MERCHANT:1:PENDING", "Merchant 1 pending", AccountType.Liability, "Merchant", "1").Result;
        _fees = _accounts.GetOrCreateAsync("SYS:FEE_REVENUE", "Fee revenue", AccountType.Revenue).Result;
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    // $100.00 ACH debit with a $1.00 fee: 10000 debit clearing, 9900 credit merchant, 100 credit fees.
    private PostingRequest AchDebit(string key = "ach-1", long gross = 10000, long fee = 100) => new(
        key, "ACH debit", "AchEntry", "entry-1",
        [
            new PostingLine(_clearing.Id, gross, EntryDirection.Debit),
            new PostingLine(_merchant.Id, gross - fee, EntryDirection.Credit),
            new PostingLine(_fees.Id, fee, EntryDirection.Credit),
        ]);

    [Fact]
    public async Task Balanced_posting_is_saved_and_balances_are_derived()
    {
        var tx = await _posting.PostAsync(AchDebit());

        Assert.Equal(3, tx.Entries.Count);
        Assert.Equal(10000, await _posting.GetBalanceAsync(_clearing.Id));
        Assert.Equal(9900, await _posting.GetBalanceAsync(_merchant.Id));
        Assert.Equal(100, await _posting.GetBalanceAsync(_fees.Id));

        var (debits, credits) = await _posting.GetTrialBalanceAsync();
        Assert.Equal(debits, credits);
    }

    [Fact]
    public async Task Unbalanced_posting_is_rejected_and_nothing_is_saved()
    {
        var bad = AchDebit() with
        {
            Lines =
            [
                new PostingLine(_clearing.Id, 10000, EntryDirection.Debit),
                new PostingLine(_merchant.Id, 9000, EntryDirection.Credit),
            ],
        };

        await Assert.ThrowsAsync<UnbalancedTransactionException>(() => _posting.PostAsync(bad));
        Assert.Equal(0, await _db.Transactions.CountAsync());
        Assert.Equal(0, await _db.Entries.CountAsync());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-500)]
    public async Task Non_positive_amounts_are_rejected(long amount)
    {
        var bad = AchDebit() with
        {
            Lines =
            [
                new PostingLine(_clearing.Id, amount, EntryDirection.Debit),
                new PostingLine(_merchant.Id, amount, EntryDirection.Credit),
            ],
        };

        await Assert.ThrowsAsync<UnbalancedTransactionException>(() => _posting.PostAsync(bad));
    }

    [Fact]
    public async Task Single_line_posting_is_rejected()
    {
        var bad = AchDebit() with { Lines = [new PostingLine(_clearing.Id, 100, EntryDirection.Debit)] };
        await Assert.ThrowsAsync<UnbalancedTransactionException>(() => _posting.PostAsync(bad));
    }

    [Fact]
    public async Task Missing_source_document_is_rejected()
    {
        var bad = AchDebit() with { SourceId = "" };
        await Assert.ThrowsAsync<UnbalancedTransactionException>(() => _posting.PostAsync(bad));
    }

    [Fact]
    public async Task Unknown_account_is_rejected()
    {
        var bad = AchDebit() with
        {
            Lines =
            [
                new PostingLine(Guid.NewGuid(), 100, EntryDirection.Debit),
                new PostingLine(_merchant.Id, 100, EntryDirection.Credit),
            ],
        };

        await Assert.ThrowsAsync<LedgerException>(() => _posting.PostAsync(bad));
        Assert.Equal(0, await _db.Transactions.CountAsync());
    }

    [Fact]
    public async Task Replaying_the_same_request_returns_the_original_and_adds_no_rows()
    {
        var first = await _posting.PostAsync(AchDebit());
        var second = await _posting.PostAsync(AchDebit());

        Assert.Equal(first.Id, second.Id);
        Assert.Equal(1, await _db.Transactions.CountAsync());
        Assert.Equal(3, await _db.Entries.CountAsync());
        Assert.Equal(10000, await _posting.GetBalanceAsync(_clearing.Id));
    }

    [Fact]
    public async Task Same_key_with_different_content_is_a_conflict()
    {
        await _posting.PostAsync(AchDebit());

        await Assert.ThrowsAsync<IdempotencyConflictException>(
            () => _posting.PostAsync(AchDebit(gross: 20000, fee: 200)));
    }

    [Fact]
    public async Task Reversal_mirrors_the_entries_and_returns_balances_to_zero()
    {
        var original = await _posting.PostAsync(AchDebit());
        var reversal = await _posting.ReverseAsync(original.Id, "rev-1", "R01 insufficient funds");

        Assert.Equal(original.Id, reversal.ReversesTransactionId);
        Assert.Equal(6, await _db.Entries.CountAsync());   // original rows are kept, never deleted
        Assert.Equal(0, await _posting.GetBalanceAsync(_clearing.Id));
        Assert.Equal(0, await _posting.GetBalanceAsync(_merchant.Id));
        Assert.Equal(0, await _posting.GetBalanceAsync(_fees.Id));

        var (debits, credits) = await _posting.GetTrialBalanceAsync();
        Assert.Equal(debits, credits);
    }

    [Fact]
    public async Task Reversal_is_idempotent_for_the_same_key_but_only_allowed_once()
    {
        var original = await _posting.PostAsync(AchDebit());
        var first = await _posting.ReverseAsync(original.Id, "rev-1", "R01");
        var replay = await _posting.ReverseAsync(original.Id, "rev-1", "R01");

        Assert.Equal(first.Id, replay.Id);
        await Assert.ThrowsAsync<LedgerException>(() => _posting.ReverseAsync(original.Id, "rev-2", "R01 again"));
        Assert.Equal(2, await _db.Transactions.CountAsync());
    }

    [Fact]
    public async Task A_reversal_cannot_be_reversed()
    {
        var original = await _posting.PostAsync(AchDebit());
        var reversal = await _posting.ReverseAsync(original.Id, "rev-1", "R01");

        await Assert.ThrowsAsync<LedgerException>(() => _posting.ReverseAsync(reversal.Id, "rev-2", "oops"));
    }

    [Fact]
    public async Task Saved_transactions_and_entries_cannot_be_modified_or_deleted()
    {
        var tx = await _posting.PostAsync(AchDebit());

        var tracked = await _db.Transactions.Include(t => t.Entries).SingleAsync(t => t.Id == tx.Id);
        _db.Entry(tracked).Property(t => t.Description).CurrentValue = "tampered";
        await Assert.ThrowsAsync<InvalidOperationException>(() => _db.SaveChangesAsync());
        _db.ChangeTracker.Clear();

        var entry = await _db.Entries.FirstAsync();
        _db.Entries.Remove(entry);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _db.SaveChangesAsync());
    }

    [Fact]
    public async Task Account_service_returns_the_existing_account_and_rejects_a_type_mismatch()
    {
        var again = await _accounts.GetOrCreateAsync("SYS:FEE_REVENUE", "Fee revenue", AccountType.Revenue);
        Assert.Equal(_fees.Id, again.Id);

        await Assert.ThrowsAsync<LedgerException>(
            () => _accounts.GetOrCreateAsync("SYS:FEE_REVENUE", "Fee revenue", AccountType.Asset));
    }
}

internal static class TestExtensions
{
    public static T Also<T>(this T value, Action<T> action)
    {
        action(value);
        return value;
    }
}
