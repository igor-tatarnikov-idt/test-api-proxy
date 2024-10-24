using Microsoft.AspNetCore.Mvc;
using RestAdapter.Const;

namespace RestAdapter.Controllers;

[ApiController]
[Route("transactions-insecure")]
public class InsecureTransactionsController  (IHttpClientFactory httpClientFactory)
    : BaseTransactionsController(httpClientFactory, HttpClientName.Insecure)
{
}