using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;

namespace DPBack.Application.Services;

public class PayUService : IPaymentService
{
    private readonly IPaymentTokenProvider _tokenProvider;
    private readonly HttpClient _client;

    public PayUService(IPaymentTokenProvider tokenProvider, HttpClient client)
    {
        _tokenProvider = tokenProvider;
        _client = client;
    }

    public async Task<string> CreatePayment(string orderId)
    {
        var token = await _tokenProvider.GetToken();

       
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false // <-- отключаем автоматические редиректы
        };

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://secure.snd.payu.com")
        };
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var payuOrder = new
        {
            continueUrl =$"http://localhost:3000/payment/{orderId}/status",
            notifyUrl = "https://nontheologically-catapultic-everett.ngrok-free.dev/api/payments/notify",
            customerIp = "127.0.0.1",
            merchantPosId = "300746",
            description ="test order",
            currencyCode = "PLN",
            totalAmount = "100", // в копейках: 1 PLN = 100
            extOrderId = orderId,
            products = new[]
            {
                new { name = "Order", unitPrice = "100", quantity = "1" } // unitPrice тоже в копейках
            }
        };

        var response = await client.PostAsJsonAsync("/api/v2_1/orders", payuOrder);

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("RAW RESPONSE: " + content);
        
        var result = JsonSerializer.Deserialize<PayUOrderResponseDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (result == null || string.IsNullOrEmpty(result.RedirectUri))
            throw new Exception("RedirectUri not found in PayU response");

        return result.RedirectUri;
    }
}

