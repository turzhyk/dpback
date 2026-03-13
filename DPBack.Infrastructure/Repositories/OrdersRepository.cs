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

        public async Task<Order> GetWithId(Guid id)
        {
            var orderEntity =
                await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.Items)
                    .Include(o => o.History)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
            if (orderEntity == null)
                throw new Exception($"Order with id {id} not found");
            return MapToOrder(orderEntity);
        }

        public async Task<List<Order>> GetAll()
        {
            var orderEntities =
                await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.Items)
                    .Include(o => o.History)
                    .Take(30)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();


            return orderEntities.Select(MapToOrder).ToList();
        }

        public async Task<Guid> Create(Order order)
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
            await _context.Orders.AddAsync(orderEntity);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                throw new Exception($"No order found with id {orderId}");
            order.PaymentStatus = status;
            await _context.SaveChangesAsync();
        }

        public async Task<OrderPaymentStatus> GetPaymentStatus(Guid orderId)
        {
            var order = await _context.Orders.AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                throw new Exception($"No order found with id {orderId}");
            return order.PaymentStatus;
        }
    
    

    public async Task ChangeStatus(Guid orderId, string author, OrderStatus status, string newAuthor)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);
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
            await _context.SaveChangesAsync();
        }

        public async Task AssignOrderWithStatus(
            Guid orderId,
            string author,
            OrderHistoryElement historyElement)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

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

            await _context.SaveChangesAsync();
        }


        public async Task Update(Order order)
        {
            var orderEntity =
                await _context.Orders.Include(o => o.History).FirstOrDefaultAsync(o => o.Id == order.Id);
            if (orderEntity == null)
                throw new Exception("Order not found");
            orderEntity.History.Add(new OrderHistoryElementEntity
            {
                Order = orderEntity,
                Status = order.History.Last().Status,
                AuthorLogin = order.History.Last().AuthorLogin,
                ChangedAt = order.History.Last().ChangedAt,
            });
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> Update(Guid id, string description, decimal price, string assignedTo)
        {
            await _context.Orders.Where(o => o.Id == id).ExecuteUpdateAsync(i => i
                .SetProperty(o => o.Descriprion, o => description)
                .SetProperty(o => o.TotalPrice, o => price)
                .SetProperty(o => o.AssignedTo, o => assignedTo));
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Orders.Where(o => o.Id == id).ExecuteDeleteAsync();
            return id;
        }
    }
}