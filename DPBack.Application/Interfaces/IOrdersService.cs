

using DPBack.Application.Contracts;
using DPBack.Domain.Models;

namespace DPBack.Application.Interfaces
{
    public interface IOrdersService
    {
        Task<Guid> CreateOrder(OrdersRequest order);
        Task<List<Order>> GetAllOrders();
        Task AssignToAsync(Guid orderId, string author);

        Task ChangeStatus(Guid orderId, string author, OrderStatus newStatus);
    }
}