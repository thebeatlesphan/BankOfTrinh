using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Features.Accounts.CreateBankAccount;
using BankOfTrinh.Server.Features.Accounts.GetBankAccount;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Server.Features.Accounts.GetCustomerBankAccounts;

public sealed class GetCustomerBankAccountsService
{
    private readonly BankDbContext _dbContext;

    public GetCustomerBankAccountsService(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetCustomerBankAccountsResponse> GetByIdAsync(
        Guid customerId,
        CancellationToken cancellationToken)
    {
        var customerExists = await _dbContext.Customers
            .AnyAsync(
            customer => customer.Id == customerId,
            cancellationToken);

        if (!customerExists)
        {
            throw new CustomerNotFoundException(customerId);
        }

        var accounts = await _dbContext.BankAccounts
            .AsNoTracking()
            .Where(account => account.CustomerId == customerId)
            .Select(account => new GetBankAccountResponse(
                account.Id,
                account.CustomerId,
                account.AccountNumber,
                account.Balance,
                account.CreatedAtUtc))
            .ToArrayAsync(cancellationToken);

        return new GetCustomerBankAccountsResponse(
            customerId,
            accounts);
    }
}