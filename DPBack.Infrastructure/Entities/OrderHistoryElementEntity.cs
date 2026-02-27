using DPBack.Infrastructure.Entities;

namespace DPBack.Infrastructure.Entities;


public class OrderHistoryElementEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string Status { get; set; }
    public DateTime ChangedAt { get; set; }
    public string AuthorLogin { get; set; }
    public OrderEntity Order { get; set; }
}