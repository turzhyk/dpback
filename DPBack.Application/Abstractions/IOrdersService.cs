

using DPBack.Application.Contracts;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions
{
    public interface IOrdersService
    {
        Task<CreateOrderResponseDto> CreateOrder(Guid userId,CreateOrderRequestDto createOrder, CancellationToken cToken);
        Task<List<OrderResponseDto>> GetAllOrders(CancellationToken cToken);
        Task<PagedRespose<OrderResponseDto>> GetOrdersFiltered(OrdersFilteredRequestDto request, CancellationToken cToken);
        Task<OrderResponseDto> GetOrderById(Guid userId,Guid orderId, CancellationToken cToken);
        Task AssignToAsync(Guid orderId, string author, CancellationToken cToken);
        Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status, CancellationToken cToken);
        Task<OrderPaymentStatus> GetPaymentStatus(Guid orderId, CancellationToken cToken);

        Task ChangeStatus(Guid orderId, string author, OrderStatus newStatus, CancellationToken cToken);
    }
}