namespace RestProvider.Models;

public class Transaction
{
    public string Id { get; set; } = null!;
    public decimal Amount { get; set; }
    public string SenderName { get; set; } = null!;
    public string RecipientName { get; set; } = null!;
    public string RecipientBankAccountNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
}