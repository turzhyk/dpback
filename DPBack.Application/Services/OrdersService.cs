using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
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
        private readonly IPriceCalcService _priceService;

        private OrderResponseDto OrderToDto(Order o)
        {
            return new OrderResponseDto
            {
                id = o.Id,
                OrderNumber = o.OrderNumber,
                Desc = o.Description,
                Price = o.TotalPrice,
                Items = o.Items.Select(i => new OrderItemResponse
                {
                    Quantity = i.Quantity,
                    Type = i.Type,
                    Options = i.Options
                }).ToList(),
                History = o.History.Select(h => new OrderHistoryElementResponse
                {
                    Status = h.Status.ToString(),
                    ChangedAt = h.ChangedAt,
                    AuthorId = h.AuthorLogin
                }).ToList(),
                AssignedTo = o.AssignedTo,
                CreatedAt = o.CreatedAt,
                IsSuspended = o.IsSuspended,
                Status = o.Status,
                PaymentStatus = o.PaymentStatus
            };
        }

        public OrdersService(IOrdersRepository ordersRepo, IPaymentService paymentService, IPriceCalcService priceCalcService
        )
        {
            _repo = ordersRepo;
            _paymentService = paymentService;
            _priceService = priceCalcService;
        }

        public async Task<string> GetOrderStatus(Guid orderId)
        {
            var status = await _repo.GetPaymentStatus(orderId);
            return status.ToString();
        }

        public async Task<List<OrderResponseDto>> GetAllOrders()
        {
            var orders = await _repo.GetAll(0, 100);
            var response = orders.Select(o =>
                OrderToDto(o)).ToList();
            return response;
        }

        public async Task<PagedRespose<OrderResponseDto>> GetOrdersFiltered(OrdersFilteredRequestDto request)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            
            var orders = await _repo.GetAll(skip, request.PageSize);
            
            
            var totalCount = await _repo.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
            return new PagedRespose<OrderResponseDto>
            {
                Items = orders.Select(OrderToDto).ToList(),
                TotalItems = totalCount,
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }

        public async Task<OrderResponseDto> GetOrderById(Guid userId,Guid orderId)
        {
            var order = await _repo.GetWithId(orderId);
            return OrderToDto(order);
        }
        public async Task<CreateOrderResponseDto> CreateOrder(CreateOrderRequestDto requestDto)
        {
            var items = requestDto.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                Quantity = i.Quantity,
                Type = i.Type,
                Options = i.Options,
            }).ToList();
            var totalPrice = _priceService.Calculate(requestDto.Items[0]);
            var (order, error) = Order.Create(
                Guid.NewGuid(),
                0,
                requestDto.Desc,
                19,
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

        public async Task<OrderPaymentStatus> GetPaymentStatus(Guid orderId)
        {
            var order = await _repo.GetWithId(orderId);
            if (order == null)
                throw new Exception("Can't modify order that is assigned to another worker");
            return order.PaymentStatus;
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