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
            .WithTags("Customers")
            .WithSummary("Get customer by Id.")
            .Produces<GetCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}