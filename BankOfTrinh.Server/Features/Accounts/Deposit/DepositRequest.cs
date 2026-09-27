using System.ComponentModel.DataAnnotations;

namespace BankOfTrinh.Server.Features.Accounts.Deposit;

public sealed record DepositRequest(
    [property: Range(typeof(decimal), "0.1", "79228162514264337593543950335")]
    decimal Amount);