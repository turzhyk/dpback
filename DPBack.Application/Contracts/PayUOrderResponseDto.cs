using System.Text.Json.Serialization;

namespace DPBack.Application.Contracts;

public class PayUOrderResponseDto
{
    public string OrderId { get; set; }
    [JsonPropertyName("redirectUri")]
    public string RedirectUri { get; set; }
    public string Status { get; set; }
    public string ExtOrderId { get; set; }
}