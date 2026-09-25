namespace BankOfTrinh.Server.Features.Customers.CreateCustomer;

public sealed class CustomerEmailAlreadyExistsException : Exception
{
    public CustomerEmailAlreadyExistsException(string email)
        : base($"A customer with email '{email}' already exists.")
    {

    }
}