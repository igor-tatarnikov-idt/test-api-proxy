using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestProvider.Models;

namespace RestProvider.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController(IMemoryCache memoryCache) : Controller
{
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        if (memoryCache.TryGetValue(GetCacheKey(id), out Transaction? transaction))
        {
            return new JsonResult(transaction)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        return new NotFoundResult();
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateTransactionRequest request)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Amount = request.Amount,
            SenderName = request.SenderName,
            RecipientName = request.RecipientName,
            RecipientBankAccountNumber = request.RecipientBankAccountNumber,
            Status = "Created"
        };
        var cacheKey = GetCacheKey(transaction.Id);
        memoryCache.Set(cacheKey, transaction, TimeSpan.FromMinutes(30));

        var result = new JsonResult(new CreateTransactionResponse
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            SenderName = transaction.SenderName,
            RecipientName = transaction.RecipientName,
            RecipientBankAccountNumber = transaction.RecipientBankAccountNumber
        })
        {
            StatusCode = StatusCodes.Status201Created
        };

        return result;
    }
    
    private static string GetCacheKey(string transactionId)
        => $"Transaction_{transactionId}";
}