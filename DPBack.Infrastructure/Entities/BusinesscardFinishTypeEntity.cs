using DPBack.Domain.Enums;


namespace DPBack.Infrastructure.Entities;

public class BusinesscardFinishTypeEntity
{
    public Guid Id { get; set; }
    public BusinesscardPaperCoating Type { get; set; } 
    public decimal Price { get; set; }
}