using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Tokenizer.Controllers;

[ApiController]
public class TokensController(IMemoryCache memoryCache) : Controller
{
    [HttpPost("tokenize")]
    public string Tokenize([FromBody] string value)
    {
        var token = GetToken(value);

        memoryCache.Set(token, value, TimeSpan.FromMinutes(30));

        return token;
    }

    [HttpGet("validate/{token}")]
    public bool Validate([FromRoute] string token)
    {
        return memoryCache.TryGetValue(token, out _);
    }
    
    [HttpPost("detokenize")]
    public string? Detokenize([FromBody] string token)
    {
        return memoryCache.TryGetValue(token, out var value)
            ? (string)value!
            : null;
    }

    private static string GetToken(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var data = SHA512.HashData(Encoding.UTF8.GetBytes(value));
        var sBuilder = new StringBuilder();
        foreach (var t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }
        return sBuilder.ToString()[..14];
    }
}