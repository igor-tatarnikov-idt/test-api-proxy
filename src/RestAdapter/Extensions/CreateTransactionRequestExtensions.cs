using RestAdapter.Models;
using RestAdapter.Models.Provider;

namespace RestAdapter.Extensions;

public static class CreateTransactionRequestExtensions
{
    public static CreateRequest ToProviderRequest(this CreateTransactionRequest request)
    {
        return new CreateRequest
        {
            Amount = request.Amount,
            RecipientBankAccountNumber = request.RecipientBankAccountNumber,
            RecipientName = request.RecipientName,
            SenderName = request.SenderName
        };
    }
}