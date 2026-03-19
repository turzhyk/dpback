using System.Text.Json;
using System.Text.Json.Nodes;
using DPBack.Domain.Enums;

namespace DPBack.Domain.Models;


public class OrderItem
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public OrderItemType Type { get; set; }
    public JsonElement Options { get; set; }
}