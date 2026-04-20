using System.Text.Json;
using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;

public record OrderItemRequest(
    int Quantity,
    OrderItemType Type,
    JsonElement Options
);