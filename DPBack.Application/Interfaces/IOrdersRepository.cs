
using DPBack.Domain.Models;

namespace DPBack.Application.Interfaces
{
    public interface IOrdersRepository
    {
        Task<Guid> Create(Order order);
        Task<Guid> Delete(Guid id);
        Task<List<Order>> GetAll( );
        Task<Order> GetWithId(Guid id);
        Task Update(Order order);
        Task<Guid> Update(Guid id, string description, decimal price, string assignedTo);
        Task ChangeStatus(Guid orderId, string author, OrderStatus status, string newAuthor);
        Task AssignOrderWithStatus(
            Guid orderId,
            string author,
            OrderHistoryElement historyElement);
    }
}