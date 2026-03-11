using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public class ChangeOrderStatusRequestDto
{
    public OrderStatus Status { get; set; }
}