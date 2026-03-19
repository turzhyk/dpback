using System.Reflection.PortableExecutable;
using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Options.Pricing;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;
using Microsoft.Extensions.Options;


namespace DPBack.Application.Pricing.Calculators;

public class BusinesscardCalculator : IPriceCalculator
{
    private readonly BusinesscardPricing _pricing;
    public OrderItemType Type => OrderItemType.Businesscard;

    public BusinesscardCalculator(IOptions<BusinesscardPricing> pricing)
    {
        _pricing = pricing.Value;
    }

    public Task<decimal> Calculate(JsonElement configJson)
    {
        Console.WriteLine(configJson);
        var config = JsonSerializer.Deserialize<BusinesscardConfig>(configJson.GetRawText());
        if (config == null)
            throw new Exception("Invalid businesscard configurations");
        // Console.WriteLine(_pricing.ThicknessPrices[0]);
        decimal price = _pricing.BasePrice ;
        price += _pricing.ThicknessPrices.GetValueOrDefault(config.Thickness);
        price += _pricing.CoatingPrices.GetValueOrDefault(config.Coating);
        return Task.FromResult(price);
    }
}