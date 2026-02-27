using DPBack.Application.Interfaces;
using DPBack.Domain.Models;


namespace DPBack.Application.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _repo;

        public OrdersService(IOrdersRepository ordersRepo)
        {
            _repo = ordersRepo;
        }

        public async Task<List<Order>> GetAllOrders() => await _repo.GetAll();
        public async Task<Guid> CreateOrder(Order order) => await _repo.Create(order);

        public async Task AssignToAsync(Guid orderId, string author)
        {
            await _repo.AssignOrderWithStatus(
                orderId,
                author,
                new OrderHistoryElement
                {
                    Status = "OrderStatus.InProgress",
                    AuthorLogin = author
                });
        }

    }
}