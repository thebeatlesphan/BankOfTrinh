namespace BankOfTrinh.Ledger;

public enum AccountType
{
    /// <summary>Normal balance is a debit (e.g. settlement clearing, cash at the ODFI).</summary>
    Asset = 1,
    /// <summary>Normal balance is a credit (e.g. merchant balances we owe).</summary>
    Liability = 2,
    /// <summary>Normal balance is a credit (e.g. fee revenue).</summary>
    Revenue = 3,
    /// <summary>Normal balance is a debit (e.g. returns write-offs, bank fees).</summary>
    Expense = 4,
}

public enum EntryDirection
{
    Debit = 1,
    Credit = 2,
}

public class Account
{
    public Guid Id { get; init; }

    /// <summary>Stable unique code, e.g. "SYS:SETTLEMENT_CLEARING" or "MERCHANT:{id}:PENDING".</summary>
    public string Code { get; init; } = "";

    public string Name { get; init; } = "";
    public AccountType Type { get; init; }

    /// <summary>Optional owner, e.g. OwnerType "Merchant" and OwnerId the merchant id. Null for system accounts.</summary>
    public string? OwnerType { get; init; }
    public string? OwnerId { get; init; }

    public string Currency { get; init; } = "USD";
    public DateTime CreatedUtc { get; init; }
}

/// <summary>
/// One balanced money movement. Immutable once saved: corrections are made with a reversing transaction.
/// </summary>
public class LedgerTransaction
{
    public Guid Id { get; init; }

    /// <summary>Caller-supplied key that makes posting safe to retry. Unique.</summary>
    public string IdempotencyKey { get; init; } = "";

    public string Description { get; init; } = "";

    /// <summary>The source document that justifies this entry, e.g. "AchEntry", "NachaFile", "ReturnFile", "Payment".</summary>
    public string SourceType { get; init; } = "";
    public string SourceId { get; init; } = "";

    /// <summary>Set when this transaction reverses another one. At most one reversal per original.</summary>
    public Guid? ReversesTransactionId { get; init; }

    public DateTime CreatedUtc { get; init; }

    public List<LedgerEntry> Entries { get; init; } = [];
}

public class LedgerEntry
{
    public Guid Id { get; init; }
    public Guid TransactionId { get; init; }
    public LedgerTransaction? Transaction { get; init; }

    public Guid AccountId { get; init; }
    public Account? Account { get; init; }

    /// <summary>Always positive whole cents. The sign comes from <see cref="Direction"/>.</summary>
    public long AmountCents { get; init; }

    public EntryDirection Direction { get; init; }
}
