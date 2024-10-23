using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RemittanceGateway.Const;
using RemittanceGateway.Models;

namespace RemittanceGateway.Controllers;

[ApiController]
[Route("transactions-secure")]
public class TransactionsController(IHttpClientFactory httpClientFactory)
    : BaseTransactionsController(httpClientFactory, HttpClientName.Secure)
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken token)
    {
        var uri = new Uri($"transactions/{id}", UriKind.Relative);
        var response = await ProviderClient.GetAsync(uri, token);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new NotFoundResult();
        }

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(token);
        var transaction = JsonSerializer.Deserialize<Transaction>(responseBody, SerializerOptions);

        return new JsonResult(transaction);
    }
}