

using DPBack.Application.Contracts;

namespace DPBack.Application.Interfaces;

public interface IPriceCalcService
{
     Task<PriceResultDto> CalculatePrice(GetPriceDto request);

}