using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Server.Features.Accounts.CreateBankAccount;

public sealed class CreateBankAccountService
{
    private readonly BankDbContext _dbContext;

    public CreateBankAccountService(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateBankAccountResponse> CreateAsync(
        Guid customerId,
        CancellationToken cancellationToken)
    {
        var customerExists = await _dbContext.Customers
            .AnyAsync(
            customer => customer.Id == customerId, cancellationToken);

        if (!customerExists)
        {
            throw new CustomerNotFoundException(customerId);
        }

        var account = new BankAccount(customerId);

        _dbContext.BankAccounts.Add(account);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateBankAccountResponse(
            account.Id,
            account.CustomerId,
            account.AccountNumber,
            account.Balance,
            account.CreatedAtUtc);
    }
}