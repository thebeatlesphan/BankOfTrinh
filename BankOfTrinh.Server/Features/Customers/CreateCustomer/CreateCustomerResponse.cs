using System;

namespace BankOfTrinh.Server.Features.Customers.CreateCustomer;

public sealed record CreateCustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedAt);