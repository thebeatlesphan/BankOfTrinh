namespace BankOfTrinh.Ledger;

public class LedgerException(string message) : Exception(message);

/// <summary>Debits and credits do not match, or the lines are otherwise not a valid posting.</summary>
public sealed class UnbalancedTransactionException(string message) : LedgerException(message);

/// <summary>The idempotency key was already used for a different request.</summary>
public sealed class IdempotencyConflictException(string key)
    : LedgerException($"Idempotency key '{key}' was already used with different content.");
