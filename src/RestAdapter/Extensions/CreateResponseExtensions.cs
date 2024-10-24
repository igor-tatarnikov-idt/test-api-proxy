using RestAdapter.Models;
using RestAdapter.Models.Provider;

namespace RestAdapter.Extensions;

public static class CreateResponseExtensions
{
    public static CreateTransactionResponse ToAdapterResponse(this CreateResponse response)
    {
        return new CreateTransactionResponse
        {
            Amount = response.Amount,
            Id = response.Id,
            RecipientName = response.RecipientName,
            RecipientBankAccountNumber = response.RecipientBankAccountNumber,
            SenderName = response.SenderName,
            Status = response.Status
        };
    }
}