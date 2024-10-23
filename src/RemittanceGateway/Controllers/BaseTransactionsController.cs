using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RemittanceGateway.Models;

namespace RemittanceGateway.Controllers;

public abstract class BaseTransactionsController(
    IHttpClientFactory httpClientFactory,
    string httpClientName)
    : Controller
{
    protected readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected readonly HttpClient ProviderClient = httpClientFactory.CreateClient(httpClientName);
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest createRequest, CancellationToken cancellationToken)
    {
        var uri = new Uri("transactions", UriKind.Relative);
        var response = await ProviderClient.PostAsJsonAsync(uri, createRequest, cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var createdTransaction = JsonSerializer.Deserialize<Transaction>(responseBody, SerializerOptions);

        return new JsonResult(createdTransaction);
    }
}