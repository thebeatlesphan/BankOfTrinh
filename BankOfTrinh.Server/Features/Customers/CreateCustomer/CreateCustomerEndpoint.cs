namespace BankOfTrinh.Server.Features.Customers.CreateCustomer;

public static class CreateCustomerEndpoint
{
    public static IEndpointRouteBuilder MapCreateCustomerEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/api/customers",
            async (
                CreateCustomerRequest request,
                CreateCustomerService service,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var response = await service.CreateAsync(
                        request,
                        cancellationToken);

                    return Results.Created(
                        $"/api/customers/{response.Id}",
                        response);
                }
                catch (CustomerEmailAlreadyExistsException exception)
                {
                    return Results.Conflict(new
                    {
                        error = exception.Message
                    });
                }
            })
            .WithName("CreateCustomer")
            .WithTags("Customers");

        return endpoints;
    }
}