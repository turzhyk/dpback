using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Options.Pricing;
using DPBack.Domain.Enums;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Pricing.Calculators;

public class PhotoCalculator:IPriceCalculator
{
    private readonly PhotoPricing _pricing;
    public OrderItemType Type => OrderItemType.PhotoA4;

    public PhotoCalculator(IOptions<PhotoPricing> pricing)
    {
        _pricing = pricing.Value;
    }

    public decimal Calculate(JsonElement config)
    {
        decimal price = _pricing.BasePrice ;
        return price;
    }
}