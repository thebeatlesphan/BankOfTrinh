using BankOfTrinh.Server.Features.Accounts.GetBankAccount;

namespace BankOfTrinh.Server.Features.Accounts.Deposit;

public static class DepositEndpoint
{
    public static IEndpointRouteBuilder MapDepositEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/api/accounts/{accountNumber}/deposits",
            async (
                string accountNumber,
                DepositRequest request,
                DepositService service,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    return Results.BadRequest(new
                    {
                        Message = "Account number is required."
                    });
                }

                if (request.Amount <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Message = "Deposit amount must be greater than zero."
                    });
                }

                try
                {
                    var response = await service.DepositAsync(
                        accountNumber,
                        request.Amount,
                        cancellationToken);

                    return Results.Ok(response);
                }
                catch (BankAccountNotFoundException)
                {
                    return Results.NotFound(new
                    {
                        Message = $"Bank account '{accountNumber}' was not found."
                    });
                }
            })
            .WithName("DepositIntoBankAccount")
            .WithTags("BankAccounts")
            .WithSummary("Deposit amount into bank account.")
            .Produces<DepositResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesValidationProblem();

        return endpoints;
    }
}