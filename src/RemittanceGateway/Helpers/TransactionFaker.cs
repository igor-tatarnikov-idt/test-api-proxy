using Bogus;
using RemittanceGateway.Models;

namespace RemittanceGateway.Helpers;

public sealed class TransactionFaker : Faker<Transaction>
{
    public TransactionFaker()
    {
        RuleFor(o => o.Amount, f => f.Finance.Amount(42, 999));
        RuleFor(o => o.SenderName, f => f.Name.FullName());
        RuleFor(o => o.RecipientName, f => f.Name.FullName());
        RuleFor(o => o.RecipientBankAccountNumber, f => f.Finance.Account(14));
    }
}