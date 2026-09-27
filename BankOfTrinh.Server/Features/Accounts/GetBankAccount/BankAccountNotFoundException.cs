namespace BankOfTrinh.Server.Features.Accounts.GetBankAccount;

public sealed class BankAccountNotFoundException : Exception
{
    public BankAccountNotFoundException(Guid accountId)
        : base($"Bank account id '{accountId}' was not found.")
    {
        AccountId = accountId;
    }

    public BankAccountNotFoundException(string accountNumber)
        : base($"Bank account number '{accountNumber}' was not found.")
    {
        AccountNumber = accountNumber;
    }

    public Guid? AccountId { get; private set; }

    public string? AccountNumber { get; private set; }
}