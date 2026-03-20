

using DPBack.Application.Contracts;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions
{
    public interface IOrdersService
    {
        Task<CreateOrderResponseDto> CreateOrder(CreateOrderRequestDto createOrder);
        Task<List<OrderResponseDto>> GetAllOrders();
        Task<PagedRespose<OrderResponseDto>> GetOrdersFiltered(OrdersFilteredRequestDto request);
        Task<OrderResponseDto> GetOrderById(Guid userId,Guid orderId);
        Task AssignToAsync(Guid orderId, string author);
        Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status);
        Task<OrderPaymentStatus> GetPaymentStatus(Guid orderId);

        Task ChangeStatus(Guid orderId, string author, OrderStatus newStatus);
    }
}