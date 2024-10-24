namespace RestAdapter.Models.Provider;

public class CreateRequest
{
    public required decimal Amount { get; set; }
    public required string SenderName { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientBankAccountNumber { get; set; }
}