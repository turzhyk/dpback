using System.Text.Json;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IPriceCalculator
{
    public OrderItemType Type { get; }
    public decimal Calculate(JsonElement config);
}