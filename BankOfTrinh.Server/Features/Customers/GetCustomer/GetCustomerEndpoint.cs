namespace BankOfTrinh.Server.Features.Customers.GetCustomer;

public static class GetCustomerEndPoint
{
    public static IEndpointRouteBuilder MapGetCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/api/customers/{id:guid}",
            async (
                Guid id,
                GetCustomerService service,
                CancellationToken cancellationToken) =>
            {
                var response = await service.GetByIdAsync(
                    id,
                    cancellationToken);

                return response is null
                ? Results.NotFound()
                : Results.Ok(response);
            })
            .WithName("GetCustomer")
            .WithTags("Customers");

        return endpoints;
    }
}