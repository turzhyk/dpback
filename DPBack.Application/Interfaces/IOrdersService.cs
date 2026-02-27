

using DPBack.Domain.Models;

namespace DPBack.Application.Interfaces
{
    public interface IOrdersService
    {
        Task<Guid> CreateOrder(Order order);
        Task<List<Order>> GetAllOrders();
        Task AssignToAsync(Guid orderId, string author);
    }
}