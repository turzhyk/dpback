using System.Text.Json.Serialization;

namespace DPBack.Application.Contracts;

public class PayUOrderResponseDto
{
    public string OrderId { get; set; }
    [JsonPropertyName("redirectUri")]
    public string RedirectUri { get; set; }
    public PayUOrderResponseStatusDto Status { get; set; }
    public string ExtOrderId { get; set; }
}

public class PayUOrderResponseStatusDto
{
    [JsonPropertyName("statusCode")]
    public string StatusCode { get; set; }
}