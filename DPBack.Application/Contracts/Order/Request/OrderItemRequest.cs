using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;

public class OrderItemRequest
{
    public int Quantity { get; set; }
    public OrderItemType Type { get; set; }
    public decimal PricePerUnit { get; set; }
    public string Options { get; set; }
}
