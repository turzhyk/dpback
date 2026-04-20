using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public record ChangeOrderStatusRequestDto(OrderStatus Status );