namespace BankOfTrinh.Server.Features.Accounts.GetBankAccount;

public sealed class BankAccountNotFoundException : Exception
{
    public BankAccountNotFoundException(Guid accountId) : base($"Bank account '{accountId}' was not found.")
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; private set; }
}