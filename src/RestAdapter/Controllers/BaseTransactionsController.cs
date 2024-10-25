using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestAdapter.Extensions;
using RestAdapter.Models;
using RestAdapter.Models.Provider;

namespace RestAdapter.Controllers;

public class BaseTransactionsController(
    IHttpClientFactory httpClientFactory,
    ILogger<BaseTransactionsController> logger,
    string httpClientName) : Controller
{
    protected readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected readonly HttpClient ProviderClient = httpClientFactory.CreateClient(httpClientName);
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest createRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request object: {Request}", JsonSerializer.Serialize(createRequest, SerializerOptions));
        
        var uri = new Uri("transactions", UriKind.Relative);
        var response = await ProviderClient.PostAsJsonAsync(uri, createRequest.ToProviderRequest(), cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var providerResponse = JsonSerializer.Deserialize<CreateResponse>(responseBody, SerializerOptions);

        return new JsonResult(providerResponse!.ToAdapterResponse());
    }
}