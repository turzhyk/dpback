namespace DPBack.Application.Contracts;

public record CreateOrderResponseDto(
    Guid OrderId,
    string PaymentUrl
);