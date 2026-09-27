
using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Domain.Transactions;
using BankOfTrinh.Server.Features.Accounts.GetBankAccount;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Server.Features.Accounts.Deposit;

public sealed class DepositService
{
    private readonly BankDbContext _dbContext;

    public DepositService(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DepositResponse> DepositAsync(
        string accountNumber,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var account = await _dbContext.BankAccounts
            .SingleOrDefaultAsync(
                account => account.AccountNumber == accountNumber,
                cancellationToken);

        if (account is null)
        {
            throw new BankAccountNotFoundException(accountNumber);
        }

        var transaction = AccountTransaction.CreateDeposit(
            account,
            amount);

        _dbContext.AccountTransactions.Add(transaction);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new DepositResponse(
            TransactionId: transaction.Id,
            AccountNumber: account.AccountNumber,
            DepositedAmount: transaction.Amount,
            NewBalance: account.Balance);
    }
}