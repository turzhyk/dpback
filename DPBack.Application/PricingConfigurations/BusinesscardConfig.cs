using System.Text.Json.Serialization;

namespace DPBack.Application.PricingConfigurations;

public class BusinesscardConfig
{
    [JsonPropertyName("thickness")]
    public string Thickness { get; set; }
    [JsonPropertyName("coating")]
    public string Coating { get; set; }
    [JsonPropertyName("aversPrint")]
    public int AversPrint { get; set; }
    [JsonPropertyName("reversPrint")]
    public int ReversPrint { get; set; }
}