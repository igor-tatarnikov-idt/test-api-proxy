using Microsoft.AspNetCore.Mvc;
using RemittanceGateway.Helpers;

namespace RemittanceGateway.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController : Controller
{
    private readonly TransactionFaker _faker = new();

    [HttpPost]
    public IActionResult Create()
    {
        var transaction = _faker.Generate();

        return new JsonResult(transaction);
    }
}