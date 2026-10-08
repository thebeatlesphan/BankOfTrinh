using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Features.Accounts.GetBankAccount;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Server.Features.Transactions.GetTransactions;

public sealed class GetBankAccountTransactionsService
{
    private readonly BankDbContext _dbContext;

    public GetBankAccountTransactionsService(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetBankAccountTransactionsResponse> GetByStringAsync(
        string bankAccountNumber,
        CancellationToken cancellationToken = default)
    {
        var bankAccountId = await _dbContext.BankAccounts
            .Where(account => account.AccountNumber == bankAccountNumber)
            .Select(account => (Guid?)account.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (bankAccountId is null)
        {
            throw new BankAccountNotFoundException(bankAccountNumber);
        }

        var transactions = await _dbContext.AccountTransactions
            .AsNoTracking()
            .Where(transaction => transaction.BankAccountId == bankAccountId)
            .OrderByDescending(transaction => transaction.CreatedAt)
            .Select(transaction => new AccountTransactionResponse
            {
                Id = transaction.Id,
                Type = transaction.Type,
                Amount = transaction.Amount,
                BalanceAfterTransaction = transaction.BalanceAfterTransaction,
                CreatedAt = transaction.CreatedAt,
            })
            .ToArrayAsync(cancellationToken);

        return new GetBankAccountTransactionsResponse
        (
            bankAccountNumber,
            transactions
        );

    }

}