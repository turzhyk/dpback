using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DPBack.Application.Contracts;
using DPBack.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace DPBack.Infrastructure.Payments;

public class PayUService : IPaymentService
{
    private readonly IPaymentTokenProvider _tokenProvider;
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public PayUService(IPaymentTokenProvider tokenProvider, HttpClient client, IConfiguration configuration)
    {
        _tokenProvider = tokenProvider; 
        _client = client;
        _configuration = configuration;
    }

    public async Task<string> CreatePayment(string orderId, decimal totalPrice)
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

        string norifyUrl = _configuration["Payu:NotifyUrl"] + "/api/payments/notify";
        var payuOrder = new
        {
            continueUrl = $"http://localhost:3000/payment/{orderId}/payment",
            notifyUrl = norifyUrl,
            customerIp = "127.0.0.1",
            merchantPosId = _configuration["PayU:ClientId"],
            description = "test order",
            currencyCode = "PLN",
            totalAmount = totalPrice.ToString(), // в копейках: 1 PLN = 100
            extOrderId = orderId,
            products = new[]
            {
                new { name = "Order", unitPrice = totalPrice.ToString(), quantity = "1" } // unitPrice тоже в копейках
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

    public async Task CapturePayment(string orderId)
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
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsync($"/api/v2_1/orders/{orderId}/captures",null);
        
    }
}