using DPBack.Application.Abstractions;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository

    {
        private readonly OrderStoreDbContext _context;

        public OrdersRepository(OrderStoreDbContext context)
        {
            _context = context;
        }

        private static Order MapToOrder(OrderEntity e)
        {
            var items = e.Items.Select(i => new OrderItem
            {
                Id = i.Id,
                Quantity = i.Quantity,
                Type = i.Type,
                Options = i.Options
            }).ToList();
            var history = e.History.Select(h => new OrderHistoryElement
            {
                Id = h.Id,
                Status = h.Status,
                ChangedAt = h.ChangedAt,
                AuthorLogin = h.AuthorLogin,
                OrderId = e.Id
            }).ToList();
            var order = Order.Create(
                e.Id,
                e.OrderNumber,
                e.Descriprion,
                e.TotalPrice,
                e.CustomerId,
                items,
                e.AssignedTo,
                e.CreatedAt,
                e.IsSuspended,
                e.Status,
                e.PaymentStatus,
                history
            ).Order;


            return order;
        }

        public async Task<Order?> GetWithId(Guid id, CancellationToken cToken)
        {
            var orderEntity =
                await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.Items)
                    .Include(o => o.History)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(cToken);
            if (orderEntity == null)
                return null;
            return MapToOrder(orderEntity);
        }

        public async Task<int> Count(CancellationToken cToken)
        {
            var count = await _context.Orders
                .AsQueryable()
                .CountAsync(cToken);
            return count;
        }

        public async Task<List<Order>> GetAll(CancellationToken cToken, int skip = 0, int take = 20)
        {
            var orderEntities =
                await _context.Orders
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(take)
                    .Include(o => o.Items)
                    .Include(o => o.History)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync(cToken);

            return orderEntities.Select(MapToOrder).ToList();
        }

        public async Task<Guid> Create(Order order, CancellationToken cToken)
        {
            var initHistoryElement = new OrderHistoryElementEntity
            {
                OrderId = order.Id,
                Status = OrderStatus.New,
                ChangedAt = DateTime.UtcNow,
                AuthorLogin = "-",
                Id = Guid.NewGuid()
            };
            List<OrderHistoryElementEntity> history = new List<OrderHistoryElementEntity>();
            history.Add(initHistoryElement);
            var items = order.Items.Select(i => new OrderItemEntity
            {
                Id = i.Id,
                Quantity = i.Quantity,
                Type = i.Type,
                Options = i.Options,
            }).ToList();
            var orderEntity = new OrderEntity
            {
                Id = order.Id,
                Descriprion = order.Description,
                TotalPrice = order.TotalPrice,
                AssignedTo = order.AssignedTo,
                Items = items,
                History = history,
                CreatedAt = order.CreatedAt,
                PaymentStatus = order.PaymentStatus,
                Status = order.Status
            };
            await _context.Orders.AddAsync(orderEntity, cToken);
            await _context.SaveChangesAsync(cToken);
            return order.Id;
        }

        public async Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status, CancellationToken cToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId, cToken);
            if (order == null)
                throw new Exception($"No order found with id {orderId}");
            order.PaymentStatus = status;
            await _context.SaveChangesAsync();
        }

        public async Task<OrderPaymentStatus?> GetPaymentStatus(Guid orderId, CancellationToken cToken)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId, cToken);
            if (order == null)
                return null;
            return order.PaymentStatus;
        }


        public async Task ChangeStatus(Guid orderId, string author, OrderStatus status, string newAuthor,
            CancellationToken cToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId, cToken);
            if (order == null)
                throw new Exception($"No order found with id {orderId}");
            order.Status = status;

            order.AssignedTo = newAuthor;

            _context.OrderStatusHistories.Add(new OrderHistoryElementEntity
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                AuthorLogin = author,
                Status = status,
                ChangedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync(cToken);
        }

        public async Task AssignOrderWithStatus(
            Guid orderId,
            string author,
            OrderHistoryElement historyElement, CancellationToken cToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId, cToken);

            if (order == null)
                throw new Exception($"No order found with id {orderId}");

            order.AssignedTo = author;

            _context.OrderStatusHistories.Add(new OrderHistoryElementEntity
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                Status = historyElement.Status,
                AuthorLogin = historyElement.AuthorLogin,
                ChangedAt = historyElement.ChangedAt,
            });

            await _context.SaveChangesAsync(cToken);
        }


        public async Task Update(Order order, CancellationToken cToken)
        {
            var orderEntity =
                await _context.Orders
                    .Include(o => o.History)
                    .FirstOrDefaultAsync(o => o.Id == order.Id, cToken);
            if (orderEntity == null)
                throw new Exception("Order not found");
            orderEntity.History.Add(new OrderHistoryElementEntity
            {
                Order = orderEntity,
                Status = order.History.Last().Status,
                AuthorLogin = order.History.Last().AuthorLogin,
                ChangedAt = order.History.Last().ChangedAt,
            });
            await _context.SaveChangesAsync(cToken);
        }

        public async Task<Guid> Update(Guid id, string description, decimal price, string assignedTo,
            CancellationToken cToken)
        {
            await _context.Orders
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(i => i
                    .SetProperty(o => o.Descriprion, o => description)
                    .SetProperty(o => o.TotalPrice, o => price)
                    .SetProperty(o => o.AssignedTo, o => assignedTo), cToken);
            return id;
        }

        public async Task<Guid> Delete(Guid id, CancellationToken cToken)
        {
            await _context.Orders
                .Where(o => o.Id == id)
                .ExecuteDeleteAsync(cToken);
            return id;
        }
    }
}