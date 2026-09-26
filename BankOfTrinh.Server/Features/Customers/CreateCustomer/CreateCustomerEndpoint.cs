using System.ComponentModel.DataAnnotations;

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

                var errors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(request.FirstName))
                {
                    errors["firstName"] = ["First name is required."];
                }

                if (string.IsNullOrWhiteSpace(request.LastName))
                {
                    errors["lastName"] = ["Last name is required"];
                }

                if (string.IsNullOrWhiteSpace(request.Email) ||
                    !new EmailAddressAttribute().IsValid(request.Email))
                {
                    errors["email"] = ["A valid email is required."];
                }

                if (errors.Count > 0)
                {
                    return Results.ValidationProblem(errors);
                }

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