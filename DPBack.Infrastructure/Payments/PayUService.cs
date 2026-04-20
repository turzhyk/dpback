using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DPBack.Application.Contracts;
using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DPBack.Infrastructure.Payments;

public class PayUService : IPaymentService
{
    private readonly IPaymentTokenProvider _tokenProvider;
    private readonly HttpClient _client;
    private readonly PayUOptions _options;
    private readonly ILogger<PayUService> _logger;

    public PayUService(IPaymentTokenProvider tokenProvider, HttpClient client, IOptions<PayUOptions> options, ILogger<PayUService> logger)
    {
        _tokenProvider = tokenProvider; 
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> CreatePayment(string orderId, decimal totalPrice)
    {
        var token = await _tokenProvider.GetToken();
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false 
        };

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://secure.snd.payu.com")
        };
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        string norifyUrl = _options.NotifyUrl + "/api/payments/notify";
        var payuOrder = new
        {
            continueUrl = $"http://localhost:3000/payment/{orderId}/payment",
            notifyUrl = norifyUrl,
            customerIp = "127.0.0.1",
            merchantPosId = _options.ClientId,
            description = "test order",
            currencyCode = "PLN",
            totalAmount = totalPrice.ToString(),
            extOrderId = orderId,
            products = new[]
            {
                new { name = "Order", unitPrice = totalPrice.ToString(), quantity = "1" }
            }
        };

        var response = await client.PostAsJsonAsync("/api/v2_1/orders", payuOrder);

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("RAW RESPONSE: " + content);

        var result = JsonSerializer.Deserialize<PayUOrderResponseDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (result == null || string.IsNullOrEmpty(result.RedirectUri))
            throw new ArgumentException("RedirectUri not found in PayU response");

        return result.RedirectUri;
    }

    public async Task CapturePayment(string orderId)
    {
        var token = await _tokenProvider.GetToken();
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false 
        };

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://secure.snd.payu.com")
        };
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsync($"/api/v2_1/orders/{orderId}/captures",null);
        
    }
}