namespace BankOfTrinh.Server.Features.Customers.GetCustomer;

public sealed record GetCustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);