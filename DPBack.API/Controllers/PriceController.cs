using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;
[ApiController]
[Route("/api/price")]
public class PriceController:ControllerBase
{
    private readonly IPriceCalcService _priceCalcService;
    public PriceController(IPriceCalcService priceCalcService)
    {
        _priceCalcService = priceCalcService;
    }
    [HttpPost]
    public async Task<ActionResult<decimal>> GetPricePerUnit([FromBody] OrderItemRequest request)
    {
        return _priceCalcService.Calculate(request);
    }
    
}