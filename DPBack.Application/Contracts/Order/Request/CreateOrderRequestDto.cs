using DPBack.Application.Contracts;
namespace DPBack.Application.Contracts

{
    public class CreateOrderRequestDto
    {
        public string Desc { get; set; }
        public Guid CreatedBy { get; set; }
        public List<OrderItemRequest> Items { get; set; }
    };
}