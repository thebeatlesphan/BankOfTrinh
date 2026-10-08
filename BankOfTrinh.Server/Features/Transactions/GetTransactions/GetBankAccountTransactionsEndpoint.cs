using BankOfTrinh.Server.Features.Accounts.GetBankAccount;

namespace BankOfTrinh.Server.Features.Transactions.GetTransactions;

public static class GetBankAccountTransactionsEndpoint
{
    public static IEndpointRouteBuilder MapGetBankAccountTransactionsEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/api/accounts/{accountNumber}/transactions",
            async (
                string accountNumber,
                GetBankAccountTransactionsService service,
                CancellationToken cancellationToken) =>
            {
                try
                {

                    var response = await service.GetByStringAsync(
                        accountNumber,
                        cancellationToken);

                    return Results.Ok(response);
                }
                catch (BankAccountNotFoundException)
                {
                    return Results.Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Bank account not found");
                }
            })
            .WithName("GetBankAccountTransactions")
            .WithTags("BankAccounts")
            .WithSummary("Gets all transactions for a bank account.")
            .Produces<GetBankAccountTransactionsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}