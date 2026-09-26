using BankOfTrinh.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Server.Features.Customers.GetCustomer;

public sealed class GetCustomerService
{
    private readonly BankDbContext _dbContext;

    public GetCustomerService(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetCustomerResponse?> GetByIdAsync(
        Guid Id,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .Where(customer => customer.Id == Id)
            .Select(customer => new GetCustomerResponse(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Email))
            .SingleOrDefaultAsync(cancellationToken);
    }
}