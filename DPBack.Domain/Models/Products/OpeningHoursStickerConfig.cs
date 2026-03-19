using System.Text.Json.Serialization;
using DPBack.Domain.Enums.Products;

namespace DPBack.Domain.Models.Products;

public class OpeningHoursStickerConfig
{
    [JsonPropertyName("size")]
    public Sticker.Size Size { get; set; }
    [JsonPropertyName("foil")]
    public Sticker.Foil Foil { get; set; }
    [JsonPropertyName("template")]
    public int Template { get; set; }
}