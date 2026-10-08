namespace BankOfTrinh.Server.Features.Transactions.Deposit;

public sealed record DepositResponse(
    Guid TransactionId,
    string AccountNumber,
    decimal DepositedAmount,
    decimal NewBalance);