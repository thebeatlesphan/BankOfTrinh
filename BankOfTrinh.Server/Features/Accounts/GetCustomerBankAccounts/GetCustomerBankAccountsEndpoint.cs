using BankOfTrinh.Server.Features.Accounts.CreateBankAccount;

namespace BankOfTrinh.Server.Features.Accounts.GetCustomerBankAccounts;

public static class GetCustomerBankAccountsEndpoint
{
    public static IEndpointRouteBuilder MapGetCustomerBankAccountsEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/api/customers/{customerId}/bank-accounts",
            async (
                Guid customerId,
                GetCustomerBankAccountsService service,
                CancellationToken cancellationToken) =>
            {
                try
                {

                    var response = await service.GetByIdAsync(
                        customerId,
                        cancellationToken);

                    return Results.Ok(response);
                }
                catch (CustomerNotFoundException)
                {
                    return Results.NotFound();
                }
            })
            .WithName("GetCustomerBankAccounts")
            .WithTags("BankAccounts")
            .WithSummary("Get all bank accounts for a customer.")
            .Produces<GetCustomerBankAccountsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}