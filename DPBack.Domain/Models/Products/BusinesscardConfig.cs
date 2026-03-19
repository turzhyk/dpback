using System.Text.Json.Serialization;
using DPBack.Domain;
using DPBack.Domain.Enums.Products;

namespace DPBack.Domain.Models.Products;

public class BusinesscardConfig
{
    [JsonPropertyName("thickness")] public Businesscard.Thickness Thickness { get; set; }
    [JsonPropertyName("coating")] public Businesscard.Coating Coating { get; set; }
}