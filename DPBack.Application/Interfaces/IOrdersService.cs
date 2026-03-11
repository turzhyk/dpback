

using DPBack.Application.Contracts;
using DPBack.Domain.Models;

namespace DPBack.Application.Interfaces
{
    public interface IOrdersService
    {
        Task<CreateOrderResponseDto> CreateOrder(OrdersRequest order);
        Task<List<Order>> GetAllOrders();
        Task AssignToAsync(Guid orderId, string author);
        Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status);
        Task<string> GetOrderStatus(Guid orderId);

        Task ChangeStatus(Guid orderId, string author, OrderStatus newStatus);
    }
}