namespace RestAdapter.Models;

public class CreateTransactionResponse
{
    public required string Id { get; set; }
    public required decimal Amount { get; set; }
    public required string SenderName { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientBankAccountNumber { get; set; }
    public required string Status { get; set; }
}