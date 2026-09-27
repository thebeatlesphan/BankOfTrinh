using BankOfTrinh.Server.Features.Accounts.GetBankAccount;

namespace BankOfTrinh.Server.Features.Accounts.GetCustomerBankAccounts;

public sealed record GetCustomerBankAccountsResponse(
    Guid CustomerId,
    GetBankAccountResponse[] Accounts);

