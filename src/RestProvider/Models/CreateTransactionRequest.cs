namespace RestProvider.Models;

public class CreateTransactionRequest
{
    public decimal Amount { get; set; }
    public string SenderName { get; set; }
    public string RecipientName { get; set; }
    public string RecipientBankAccountNumber { get; set; }
}