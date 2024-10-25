using Microsoft.AspNetCore.Mvc;
using RestAdapter.Const;

namespace RestAdapter.Controllers;

[ApiController]
[Route("transactions-insecure")]
public class InsecureTransactionsController  (
    IHttpClientFactory httpClientFactory,
    ILogger<SecureTransactionsController> logger)
    : BaseTransactionsController(httpClientFactory, logger, HttpClientName.Insecure)
{
}