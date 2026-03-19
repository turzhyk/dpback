using DPBack.Application.Contracts;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IPriceCalcService
{
     Task<decimal> Calculate(OrderItemRequest request);
}