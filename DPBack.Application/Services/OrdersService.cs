using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;

using DPBack.Domain.Models;
using Microsoft.AspNetCore.SignalR;


namespace DPBack.Application.Services
{
    public class OrdersService : IOrdersService

    {
        public static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedTransitions = new()
        {
            { OrderStatus.New, new[] { OrderStatus.InProgress, OrderStatus.Cancelled } },
            {
                OrderStatus.InProgress, new[]

                    { OrderStatus.Produced, OrderStatus.InProgress }
            },
            {
                OrderStatus.Produced, new[] { OrderStatus.Packing, OrderStatus.ReadyForShipping }
            },
            {
                OrderStatus.Packing, new[] { OrderStatus.ReadyForShipping }
            },
            { OrderStatus.ReadyForShipping, new[] { OrderStatus.InDelivery } },
            { OrderStatus.InDelivery, new[] { OrderStatus.Done } },
            { OrderStatus.Done, [] }
        };

        private readonly IOrdersRepository _repo;
        private readonly IPaymentService _paymentService;

        public OrdersService(IOrdersRepository ordersRepo, IPaymentService paymentService
        )
        {
            _repo = ordersRepo;
            _paymentService = paymentService;
        }

        public async Task<string> GetOrderStatus(Guid orderId)
        {
            var status = await _repo.GetPaymentStatus(orderId);
            return status.ToString();
        }

        public async Task<List<Order>> GetAllOrders() => await _repo.GetAll();

        public async Task<CreateOrderResponseDto> CreateOrder(OrdersRequest request)
        {
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
            await _repo.Create(order);
            var paymentUrl = await _paymentService.CreatePayment(order.Id.ToString());
            return new CreateOrderResponseDto { OrderId = order.Id.ToString(), PaymentUrl = paymentUrl };
        }

        public async Task ChangeStatus(Guid orderId, string author, OrderStatus newStatus)
        {
            var order = await _repo.GetWithId(orderId);
            if (order.AssignedTo != author)
                throw new Exception("Can't modify order that is assigned to another worker");
            if (AllowedTransitions[order.Status].Contains(newStatus))
            {
                var newAuthor = newStatus == OrderStatus.InProgress ? author : "";
                await _repo.ChangeStatus(orderId, author, newStatus, newAuthor);
            }
            else
            {
                throw new Exception("");
            }
        }

        public async Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status)
        {
            await _repo.SetPaymentStatus(orderId, status);
       
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