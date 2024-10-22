namespace RemittanceGateway.Models;

public class Transaction
{
    public string Id { get; set; }
    public decimal Amount { get; set; }
    public string SenderName { get; set; }
    public string RecipientName { get; set; }
    public string RecipientBankAccountNumber { get; set; }
    public string Status { get; set; }
}