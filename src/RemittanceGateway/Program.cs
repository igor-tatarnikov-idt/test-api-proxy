var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var restProviderApiBaseUrl = builder.Configuration.GetSection("RestProvider:BaseUrl").Value;
builder.Services.AddHttpClient("RestProviderClient", client =>
{
    client.BaseAddress = new Uri(restProviderApiBaseUrl!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();