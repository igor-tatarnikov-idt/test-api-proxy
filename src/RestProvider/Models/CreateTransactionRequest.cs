namespace RestProvider.Models;

public class CreateTransactionRequest
{
    public required decimal Amount { get; set; }
    public required string SenderName { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientBankAccountNumber { get; set; }
}