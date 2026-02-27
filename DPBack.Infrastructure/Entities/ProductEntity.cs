
using DPBack.Domain.Enums;

namespace DPBack.Infrastructure.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public OrderItemType Type { get; set; }
    // public IPricingStrategy PricingStrategy { get; set;}
    
}