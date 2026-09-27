using BankOfTrinh.Server.Domain.Accounts;

namespace BankOfTrinh.Server.Domain.Transactions;

public sealed class AccountTransaction
{
    private AccountTransaction()
    {
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid BankAccountId { get; private set; }

    public BankAccount BankAccount { get; private set; } = null!;

    public TransactionType Type { get; private set; }

    public decimal Amount { get; private set; }

    public decimal BalanceAfterTransaction { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static AccountTransaction CreateDeposit(
        BankAccount account,
        decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Deposit amount must be greater than zero.");
        }

        account.Deposit(amount);

        return new AccountTransaction
        {
            BankAccountId = account.Id,
            BankAccount = account,
            Type = TransactionType.Deposit,
            Amount = amount,
            BalanceAfterTransaction = account.Balance,
            CreatedAt = DateTime.UtcNow
        };
    }
}