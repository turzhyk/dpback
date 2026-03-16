using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public class OrdersFilteredRequestDto
{
    public OrderStatus? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}