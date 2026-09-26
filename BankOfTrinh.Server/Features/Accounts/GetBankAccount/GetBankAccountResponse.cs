namespace BankOfTrinh.Server.Features.Accounts.GetBankAccount;

public sealed record GetBankAccountResponse(
    Guid Id,
    Guid CustomerId,
    string AccountNumber,
    decimal Balance,
    DateTime CreatedAtUtc);