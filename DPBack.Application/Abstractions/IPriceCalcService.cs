

using DPBack.Application.Contracts;

namespace DPBack.Application.Abstractions;

public interface IPriceCalcService
{
     Task<PriceResultDto> CalculatePrice(GetPriceDto request);

}