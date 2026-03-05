using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public class ChangeOrderStatusRequestDto
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}