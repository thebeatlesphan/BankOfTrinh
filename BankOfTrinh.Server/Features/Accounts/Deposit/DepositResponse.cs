namespace BankOfTrinh.Server.Features.Accounts.Deposit;

public sealed record DepositResponse(
    Guid TransactionId,
    string AccountNumber,
    decimal DepositedAmount,
    decimal NewBalance);