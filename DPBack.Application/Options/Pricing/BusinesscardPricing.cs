using System.Text.Json.Serialization;
using DPBack.Domain.Enums.Products;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Options.Pricing;

public class BusinesscardPricing
{
    public decimal BasePrice { get; set; }
    [JsonPropertyName("Thickness")]
    public Dictionary<Businesscard.Thickness, decimal> ThicknessPrices  {get;set;} = new();
    [JsonPropertyName("Coating")]
    public Dictionary<Businesscard.Coating, decimal> CoatingPrices { get; set; } = new();

    
}