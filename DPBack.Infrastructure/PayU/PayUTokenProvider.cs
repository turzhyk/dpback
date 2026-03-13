using System.Net.Http.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using Microsoft.Extensions.Configuration;

namespace DPBack.API.PayU;


public class PayUTokenProvider:IPaymentTokenProvider
{
    private string? _token;
    private DateTime _expires;
    private readonly IConfiguration _configuration;

    public PayUTokenProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GetToken()
    {
        if (_token != null && DateTime.UtcNow < _expires)
        return _token;

        var client = new HttpClient();

        var values = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id",  _configuration["PayU:ClientId"]},
            { "client_secret", _configuration["PayU:Secret"]}
        };

        var content = new FormUrlEncodedContent(values);

        var response = await client.PostAsync(
            "https://secure.snd.payu.com/pl/standard/user/oauth/authorize",
            content);

        var json = await response.Content.ReadFromJsonAsync<PayUTokenReponseDto>();
        Console.WriteLine(json.AccessToken);
        _token = json.AccessToken;
        _expires = DateTime.UtcNow.AddSeconds(json.ExpiresIn - 60);

        return _token;
    }
}