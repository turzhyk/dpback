using DPBack.Application.Contracts;
using DPBack.Domain.Models;

namespace DPBack.Application.Extensions;

public static class OrderExtensions
{
    public static OrderResponseDto ToDto(this Order o)
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
}