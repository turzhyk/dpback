using System.Text.Json;
using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;

public class OrderItemRequest
{
    public int Quantity { get; set; }
    public OrderItemType Type { get; set; }
    public JsonElement Options { get; set; }
}
