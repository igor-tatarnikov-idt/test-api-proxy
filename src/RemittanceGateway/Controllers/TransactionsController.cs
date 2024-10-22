using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RemittanceGateway.Helpers;
using RemittanceGateway.Models;

namespace RemittanceGateway.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly TransactionFaker _faker = new();
    private readonly HttpClient _restProviderHttpClient = httpClientFactory.CreateClient("RestProviderClient");

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken token)
    {
        var response = await _restProviderHttpClient.GetAsync($"/transactions/{id}", token);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new NotFoundResult();
        }

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(token);
        var transaction = JsonSerializer.Deserialize<Transaction>(responseBody, _serializerOptions);

        return new JsonResult(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var transactionToCreate = _faker.Generate();

        var response = await _restProviderHttpClient.PostAsJsonAsync("/transactions", transactionToCreate, cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var createdTransaction = JsonSerializer.Deserialize<Transaction>(responseBody,_serializerOptions);

        return new JsonResult(createdTransaction);
    }
}