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

var restProviderApiBaseUrl = builder.Configuration.GetSection("RestProvider:BaseUrl").Value;
builder.Services.AddHttpClient("RestProviderClient", client =>
{
    client.BaseAddress = new Uri(restProviderApiBaseUrl!);
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

app.Run();