using System.Text.Json;
using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;

public record GetPriceDto(
    OrderItemType Type,
    JsonElement Configuration
);