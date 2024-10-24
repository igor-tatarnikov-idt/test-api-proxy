using RestAdapter.Const;
using RestAdapter.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSerilog(conf =>
    conf.Enrich.FromLogContext()
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
        .WriteTo.Console(new RenderedCompactJsonFormatter()));

var providerBaseUrl = builder.Configuration.GetSection("RestProvider:BaseUrl").Value;
var providerHost = builder.Configuration.GetSection("RestProvider:Host").Value;
builder.Services.AddHttpClient(HttpClientName.Insecure, client =>
{
    client.BaseAddress = new Uri(providerBaseUrl!);
});

var proxyBaseUrl = builder.Configuration.GetSection("Proxy:BaseUrl").Value;
builder.Services.AddHttpClient(HttpClientName.Secure, client =>
{
    client.BaseAddress = new Uri(proxyBaseUrl!);
    client.DefaultRequestHeaders.Add("X-Target-Host", providerHost);
    client.DefaultRequestHeaders.Add("X-Tokenization-Mode", "detokenize");
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();
app.UseSerilogRequestLogging(options =>
    {
        // Attach additional properties to the request completion event
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        };
    }
);
app.UseMiddleware<HttpLoggingMiddleware>();

app.Run();