using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;

namespace DPBack.API.PayU;


public class PayUTokenProvider:IPaymentTokenProvider
{
    private string? _token;
    private DateTime _expires;

    public PayUTokenProvider()
    {
        
    }

    public async Task<string> GetToken()
    {
        if (_token != null && DateTime.UtcNow < _expires)
        return _token;

        var client = new HttpClient();

        var values = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id",  "300746"},
            { "client_secret", "2ee86a66e5d97e3fadc400c9f19b065d"}
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