using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Pricing;
using DPBack.Domain.Models;

namespace DPBack.Application.Services;


public class PriceCalcService : IPriceCalcService
{
    private readonly PriceCalculatorFactory _factory;

    public PriceCalcService(PriceCalculatorFactory factory)
    {
        _factory = factory;
    }

    public async Task<decimal> Calculate(OrderItemRequest request)
    {
        var item = new OrderItem { Type = request.Type, Options = request.Options };
        var calc = _factory.Get(item.Type);
        return await calc.Calculate(item.Options);
    }
}