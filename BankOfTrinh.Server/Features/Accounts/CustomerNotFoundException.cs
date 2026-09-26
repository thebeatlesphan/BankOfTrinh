namespace BankOfTrinh.Server.Features.Accounts.CreateBankAccount;

public sealed class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(Guid customerId)
        : base($"Customer '{customerId}' was not found.")
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; private set; };
}