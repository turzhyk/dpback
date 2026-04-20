using DPBack.Application.Contracts;

namespace DPBack.Application.Contracts

{
    public record CreateOrderRequestDto(
        string Desc,
        Guid CreatedBy,
        List<OrderItemRequest> Items
    );
}