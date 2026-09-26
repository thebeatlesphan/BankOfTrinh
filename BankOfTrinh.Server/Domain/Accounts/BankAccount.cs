using BankOfTrinh.Server.Domain.Customers;

namespace BankOfTrinh.Server.Domain.Accounts;

public sealed class BankAccount
{
    private BankAccount()
    {
        // Required by EF Core
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }

    public string AccountNumber { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public BankAccount(Guid customerId, string accountNumber)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        AccountNumber = accountNumber;
        Balance = 0m;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Customer Customer { get; private set; } = null!;

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException(
                "Deposit amount must be greater than zero.");

        Balance += amount;
    }

    public void WithDraw(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException(
                "Withdrawl amount must be greater than zero.");

        if (amount > Balance)
            throw new InvalidOperationException(
                "Insufficient funds.");

        Balance -= amount;
    }

    public static string GenerateAccountNumber()
    {
        return Random.Shared.NextInt64(1000000000, 9999999999)
            .ToString();
    }
}