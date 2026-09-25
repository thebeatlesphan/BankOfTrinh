using System.ComponentModel.DataAnnotations;

namespace BankOfTrinh.Server.Features.Customers.CreateCustomer;

public sealed record CreateCustomerRequest(
    [property: Required, MaxLength(100)]
    string FirstName,

    [property: Required, MaxLength(100)]
    string LastName,

    [property: Required, EmailAddress, MaxLength(320)]
    string Email);