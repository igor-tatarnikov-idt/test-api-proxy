using Microsoft.AspNetCore.Mvc;
using RestAdapter.Const;

namespace RestAdapter.Controllers;

[ApiController]
[Route("transactions-secure")]
public class SecureTransactionsController (
    IHttpClientFactory httpClientFactory,
    ILogger<SecureTransactionsController> logger)
    : BaseTransactionsController(httpClientFactory, logger, HttpClientName.Secure)
{
}