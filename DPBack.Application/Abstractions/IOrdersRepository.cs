using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions
{
    public interface IOrdersRepository
    {
        Task<Guid> Create(Order order, CancellationToken cToken);
        Task<Guid> Delete(Guid id, CancellationToken cToken);
        Task<List<Order>> GetAll(CancellationToken cToken, int skip, int take);
        Task<int> Count(CancellationToken cToken);
        Task<Order?> GetWithId(Guid id, CancellationToken cToken);
        Task Update(Order order, CancellationToken cToken);
        Task<Guid> Update(Guid id, string description, decimal price, string assignedTo, CancellationToken cToken);
        Task ChangeStatus(Guid orderId, string author, OrderStatus status, string newAuthor, CancellationToken cToken);
        Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status, CancellationToken cToken);
        Task<OrderPaymentStatus?> GetPaymentStatus(Guid orderId, CancellationToken cToken);

        Task AssignOrderWithStatus(
            Guid orderId,
            string author,
            OrderHistoryElement historyElement, CancellationToken cToken);
    }
}