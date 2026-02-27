using DPBack.Application.Contracts;
namespace DPBack.Application.Contracts

{
    public class OrdersRequest
    {
        public string Desc { get; set; }

        public decimal Price { get; set; }
        public List<OrderItemRequest> Items { get; set; }
    };
}