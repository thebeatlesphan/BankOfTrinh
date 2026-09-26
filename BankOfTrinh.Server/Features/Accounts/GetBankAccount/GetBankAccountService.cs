using BankOfTrinh.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Server.Features.Accounts.GetBankAccount;

public sealed class GetBankAccountService
{
    private readonly BankDbContext _dbContext;

    public GetBankAccountService(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetBankAccountResponse> GetByIdAsync(
        Guid accountId,
        CancellationToken cancellationToken)
    {
        var account = await _dbContext.BankAccounts
            .AsNoTracking()
            .Where(account => account.Id == accountId)
            .Select(account => new GetBankAccountResponse(
                account.Id,
                account.CustomerId,
                account.AccountNumber,
                account.Balance,
                account.CreatedAtUtc))
            .SingleOrDefaultAsync(cancellationToken);

        return account
            ?? throw new BankAccountNotFoundException(accountId);
    }
}