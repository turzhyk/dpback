
using DPBack.Domain.Models;

namespace DPBack.Infrastructure.Entities

{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public string Descriprion { get; set; }
        public bool IsSuspended { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string AssignedTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }
        
        public List<OrderItemEntity> Items { get; set; } 

        public List<OrderHistoryElementEntity> History { get; set; } = new();
    }
}
