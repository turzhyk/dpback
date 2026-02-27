
using DPBack.Domain.Enums;

namespace DPBack.Infrastructure.Entities;



public class OrderItemEntity
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public OrderItemType Type { get; set; }
    public decimal PricePerUnit { get; set; }
    public string Options { get; set; }
    
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; }
}