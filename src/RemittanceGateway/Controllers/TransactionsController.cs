using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RemittanceGateway.Const;
using RemittanceGateway.Models;

namespace RemittanceGateway.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _restAdapterClient = httpClientFactory.CreateClient(HttpClientName.Rest);
    
    [HttpPost("rest")]
    public async Task<IActionResult> CreateRest(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var uri = new Uri("transactions-secure", UriKind.Relative);
        var response = await _restAdapterClient.PostAsJsonAsync(uri, request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var createdTransaction = JsonSerializer.Deserialize<CreateTransactionResponse>(responseBody, _serializerOptions);

        return new JsonResult(createdTransaction);
    }
}