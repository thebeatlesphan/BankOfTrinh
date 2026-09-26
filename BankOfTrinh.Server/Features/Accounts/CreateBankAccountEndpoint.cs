using Microsoft.AspNetCore.Http.HttpResults;

namespace BankOfTrinh.Server.Features.Accounts.CreateBankAccount;

public static class CreateBankAccountEndpoint
{
    public static void MapCreateBankAccountEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/api/customers/{customerId:guid}/accounts",
            HandleAsync)
            .WithName("CreateBankAccount")
            .WithSummary("Creates a bank account for an existing customer")
            .Produces<CreateBankAccountResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();
    }

    public static async Task<
        Results<Created<CreateBankAccountResponse>, ProblemHttpResult>>
        HandleAsync(
        Guid customerId,
        CreateBankAccountService service,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await service.CreateAsync(
                customerId,
                cancellationToken);

            return TypedResults.Created(
                $"/api/accounts/{response.Id}",
                response);
        }
        catch (CustomerNotFoundException exception)
        {
            return TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Customer not found",
                detail: exception.Message);
        }
    }
}