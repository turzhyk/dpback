using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions
{
    public interface IOrdersRepository
    {
        Task<Guid> Create(Order order);
        Task<Guid> Delete(Guid id);
        Task<List<Order>> GetAll(int skip, int take );
        Task<int> Count();
        Task<Order?> GetWithId(Guid id);
        Task Update(Order order);
        Task<Guid> Update(Guid id, string description, decimal price, string assignedTo);
        Task ChangeStatus(Guid orderId, string author, OrderStatus status, string newAuthor);
        Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status);
        Task<OrderPaymentStatus?> GetPaymentStatus(Guid orderId);
        Task AssignOrderWithStatus(
            Guid orderId,
            string author,
            OrderHistoryElement historyElement);
    }
}