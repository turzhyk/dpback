using System.Text.Json.Serialization;
using DPBack.Domain.Enums.Products;

namespace DPBack.Application.Options.Pricing;

public class OpeningHoursStickerPricing
{
    [JsonPropertyName("Size")] public Dictionary<Sticker.Size, decimal> Size { get; set; } = new();
    public Dictionary<Sticker.Foil, decimal> Foil { get; set; } = new();
}