using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Options.Pricing;
using DPBack.Domain.Enums;
using DPBack.Domain.Enums.Products;
using DPBack.Domain.Models.Products;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Pricing.Calculators;

public class OpeningHoursStickerCalculator : IPriceCalculator
{
    private readonly OpeningHoursStickerPricing _pricing;

    public OpeningHoursStickerCalculator(IOptions<OpeningHoursStickerPricing> pricing)
    {
        _pricing = pricing.Value;
    }

    public OrderItemType Type => OrderItemType.OpeningHoursSticker;

    public Task<decimal> Calculate(JsonElement configJson)
    {
        var config = JsonSerializer.Deserialize<OpeningHoursStickerConfig>(configJson.GetRawText());
        if (config == null)
            throw new Exception("Invalid opening hours sticker configurations");
        
        
        if(!_pricing.Size.ContainsKey(config.Size)) throw new Exception($"Sticker size {config.Size} is not allowed");

        if ((int)config.Foil > Enum.GetNames(typeof(Sticker.Foil)).Length - 1)
            throw new Exception("No such foil found");
       
        var price = _pricing.Size.GetValueOrDefault(config.Size);
        price *= _pricing.Foil.GetValueOrDefault(config.Foil);
        return Task.FromResult(price);
    }
}