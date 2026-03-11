using System.Text.Json.Serialization;

namespace DPBack.Application.Contracts;

public class PayUWebhookDto
{
    [JsonPropertyName("order")] public PayUOrderDto Order { get; set; }
}

public class PayUOrderDto

{
    [JsonPropertyName("orderId")] public string OrderId { get; set; }
    [JsonPropertyName("extOrderId")] public string ExtOrderId { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; }
}