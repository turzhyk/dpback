using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;
using DPBack.Domain.Models;

// private static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedTransitions =
//     new()
//     {
//         { OrderStatus.New, new [] { OrderStatus.InProgress, OrderStatus.Cancelled } },
//         { OrderStatus.InProgress, new [] { OrderStatus.InfoNeeded, OrderStatus.Produced, OrderStatus.Cancelled } },
//         { OrderStatus.Produced, new [] { OrderStatus.Packing } },
//         { OrderStatus.Packing, new [] { OrderStatus.ReadyForShipping } },
//         { OrderStatus.ReadyForShipping, new [] { OrderStatus.InDelivery } },
//         { OrderStatus.InDelivery, new [] { OrderStatus.Done } }
//     };
// if (!AllowedTransitions[order.Status].Contains(newStatus))
//     throw new InvalidOperationException($"Cannot change status from {order.Status} to {newStatus}");
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
        public async Task<Guid> CreateOrder(OrdersRequest request) {
            var items = request.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                Quantity = i.Quantity,
                Type = i.Type,
                Options = i.Options,
            }).ToList();
            var (order, error) = Order.Create(
                Guid.NewGuid(),
                0,
                request.Desc,
                request.Price,
                items,
                "",
                DateTime.UtcNow,
                false,
                status: OrderStatus.New,
                paymentStatus: OrderPaymentStatus.Waiting,
                null
            );
          return  await _repo.Create(order);
        }

        public async Task ChangeStatus(Guid orderId, string author, OrderStatus newStatus)
        {
            try
            {

                if (newStatus == OrderStatus.New)
                    throw new Exception("Cannot set order status to NEW");
                var order = await _repo.GetWithId(orderId);
                if(order == null)
                    throw new Exception("Order not found");
                if (order.Status == OrderStatus.InDelivery && newStatus != OrderStatus.Done)
                    throw new Exception("Cannot change status of order that is already in delivery");
                if (order.Status == OrderStatus.Done)
                    throw new Exception("Cannot change status of DONE order");
                
                var newAuthor = newStatus == OrderStatus.InProgress? author : null;
                await _repo.ChangeStatus(orderId, author, newStatus, newAuthor);
            }
            catch
            {
                
            }
        }
        public async Task AssignToAsync(Guid orderId, string author)
        {
            await _repo.AssignOrderWithStatus(
                orderId,
                author,
                new OrderHistoryElement
                {
                    Status = OrderStatus.InProgress,
                    AuthorLogin = author,
                    ChangedAt = DateTime.UtcNow
                });
        }

    }
}