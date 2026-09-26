using BankOfTrinh.Server.Domain.Accounts;

namespace BankOfTrinh.Server.Domain.Customers;

public sealed class Customer
{
    private readonly List<BankAccount> _bankAccounts = [];
    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();

    private Customer()
    {
        // Required by EF Core
    }

    public Customer(string firstName, string lastName, string email)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }

}