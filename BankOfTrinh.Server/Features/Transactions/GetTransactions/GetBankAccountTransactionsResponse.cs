using BankOfTrinh.Server.Domain.Transactions;

namespace BankOfTrinh.Server.Features.Transactions.GetTransactions;

public sealed record GetBankAccountTransactionsResponse(
    string BankAccountNumber,
    AccountTransactionResponse[] Transactions
    );

public sealed class AccountTransactionResponse
{
    public Guid Id { get; set; }

    public TransactionType Type { get; set; }

    public decimal Amount { get; set; }

    public decimal BalanceAfterTransaction { get; set; }

    public DateTime CreatedAt { get; set; }
}