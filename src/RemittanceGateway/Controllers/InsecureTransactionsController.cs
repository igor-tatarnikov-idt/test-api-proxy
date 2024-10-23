using Microsoft.AspNetCore.Mvc;
using RemittanceGateway.Const;

namespace RemittanceGateway.Controllers;

[ApiController]
[Route("transactions-insecure")]
public class InsecureTransactionsController(IHttpClientFactory httpClientFactory)
    : BaseTransactionsController(httpClientFactory, HttpClientName.Insecure)
{
}