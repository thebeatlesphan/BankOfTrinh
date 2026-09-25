using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Server.Features.Customers.CreateCustomer;

public sealed class CreateCustomerService
{
    private readonly BankDbContext _dbContext;

    public CreateCustomerService(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateCustomerResponse> CreateAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailAlreadyExists = await _dbContext.Customers
            .AnyAsync(
                customer => customer.Email == normalizedEmail,
                cancellationToken);

        if (emailAlreadyExists)
        {
            throw new CustomerEmailAlreadyExistsException(normalizedEmail);
        }

        var customer = new Customer(
            request.FirstName.Trim(),
            request.LastName.Trim(),
            normalizedEmail);

        _dbContext.Customers.Add(customer);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCustomerResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.CreatedAt);
    }
}