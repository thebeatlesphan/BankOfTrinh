namespace BankOfTrinh.Server.Features.Accounts.CreateBankAccount;

public sealed record CreateBankAccountResponse(
    Guid Id,
    Guid CustomerId,
    string AccountNumber,
    decimal Balance,
    DateTime CreatedAtUtc);