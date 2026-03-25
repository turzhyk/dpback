using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Domain.Models;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<OrdersService> _logger;

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

        public OrdersService(IOrdersRepository ordersRepo, IPaymentService paymentService,
            IPriceCalcService priceCalcService
            , ILogger<OrdersService> logger)
        {
            _repo = ordersRepo;
            _paymentService = paymentService;
            _priceService = priceCalcService;
            _logger = logger;
        }


        public async Task<List<OrderResponseDto>> GetAllOrders(CancellationToken cToken)
        {
            _logger.LogInformation("Getting all orders");
            var orders = await _repo.GetAll(cToken, 0, 100);
            var response = orders.Select(o =>
                OrderToDto(o)).ToList();
            return response;
        }

        public async Task<PagedRespose<OrderResponseDto>> GetOrdersFiltered(OrdersFilteredRequestDto request,
            CancellationToken cToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            _logger.LogInformation("Requesting {pageSize} orders for page nr. {pageNumber}",
                request.PageSize,
                request.PageNumber);

            var orders = await _repo.GetAll(cToken, skip, request.PageSize);

            var totalCount = await _repo.Count(cToken);
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

        public async Task<OrderResponseDto> GetOrderById(Guid userId, Guid orderId, CancellationToken cToken)
        {
            _logger.LogInformation(
                "Getting order {orderId} for user {userId}",
                orderId,
                userId);
            var order = await _repo.GetWithId(orderId, cToken);
            if (order == null)
                throw new Exception($"Order with id {orderId} not found");
            

            return OrderToDto(order);
        }

        public async Task<CreateOrderResponseDto> CreateOrder(Guid userId, CreateOrderRequestDto requestDto,
            CancellationToken cToken)
        {
            _logger.LogInformation("Creating new order for user {userId}", userId);
            var items = requestDto.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                Quantity = i.Quantity,
                Type = i.Type,
                Options = i.Options,
            }).ToList();
            decimal totalPrice = 0;
            foreach (var i in requestDto.Items)
            {
                totalPrice += _priceService.Calculate(i);
            }

            var (order, error) = Order.Create(
                Guid.NewGuid(),
                0,
                requestDto.Desc,
                totalPrice,
                items,
                "",
                DateTime.UtcNow,
                false,
                status: OrderStatus.New,
                paymentStatus: OrderPaymentStatus.Waiting,
                null
            );
            await _repo.Create(order, cToken);
            var paymentUrl = await _paymentService.CreatePayment(order.Id.ToString(), totalPrice);
            return new CreateOrderResponseDto { OrderId = order.Id.ToString(), PaymentUrl = paymentUrl };
        }

        public async Task ChangeStatus(Guid orderId, string author, OrderStatus newStatus, CancellationToken cToken)
        {
            var order = await _repo.GetWithId(orderId, cToken);
            if (order == null)
                throw new Exception($"Order with id {orderId} not found");
            _logger.LogInformation("Changing {orderId} order status from {oldStatus} to {newStatus} by {author}",
                orderId, order.Status, newStatus, author);

            if (order.AssignedTo != author)
                throw new Exception("Order status change is not allowed");
            

            if (AllowedTransitions[order.Status].Contains(newStatus))
            {
                var newAuthor = newStatus == OrderStatus.InProgress ? author : "";
                await _repo.ChangeStatus(orderId, author, newStatus, newAuthor, cToken);
            }
            else
            {
                throw new Exception("Order status change is not allowed");
            }
        }

        public async Task<OrderPaymentStatus> GetPaymentStatus(Guid orderId, CancellationToken cToken)
        {
            _logger.LogInformation("Getting order {orderId} payment status", orderId);
            var order = await _repo.GetWithId(orderId, cToken);
            if (order == null)
                throw new Exception($"Order with id {orderId} not found");
            return order.PaymentStatus;
        }

        public async Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status, CancellationToken cToken)
        {
            var order = await _repo.GetWithId(orderId, cToken);
            if (order == null)
                throw new Exception($"Order with id {orderId} not found");
            if (order.PaymentStatus == status)
                throw new Exception($"Can't change {orderId} payment status to the same value");
            
            _logger.LogInformation("Changing order {orderId} payment status from {oldStatus} to {newStatus}",
                orderId, order.PaymentStatus, status);
            await _repo.SetPaymentStatus(orderId, status, cToken);
        }

        public async Task AssignToAsync(Guid orderId, string author, CancellationToken cToken)
        {
            var order = await _repo.GetWithId(orderId, cToken);
            if (order == null)
                throw new Exception($"Order with id {orderId} not found");
            
            await _repo.AssignOrderWithStatus(
                orderId,
                author,
                new OrderHistoryElement
                {
                    Status = OrderStatus.InProgress,
                    AuthorLogin = author,
                    ChangedAt = DateTime.UtcNow
                }, cToken);
        }
    }
}