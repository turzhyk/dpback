using System.Text.Json;
using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;

public class GetPriceDto
{
    public OrderItemType Type { get; set; }
    public JsonElement Configuration { get; set; }
}