namespace BankOfTrinh.Server.Features.Accounts.GetBankAccount;

public static class GetBankAccountEndpoint
{
    public static IEndpointRouteBuilder MapGetBankAccountEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/api/accounts/{accountId:guid}",
            async (
                Guid accountId,
                GetBankAccountService service,
                CancellationToken cancellationToken) =>
            {
                var response = await service.GetByIdAsync(
                    accountId,
                    cancellationToken);

                return response is null
                ? Results.NotFound()
                : Results.Ok(response);
            })
            .WithName("GetBankAccount")
            .WithTags("BankAccounts")
            .WithSummary("Get a bank account by ID.")
            .Produces<GetBankAccountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}