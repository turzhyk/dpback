using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;

[Route("api/delivery")]
[ApiController]
public class DeliveryController:ControllerBase
{
    private IOrdersService _ordersService;
    public DeliveryController( IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }
    [HttpGet("list")]
    public async Task<ActionResult<List<DeliveryOptionResposeDto>>> GetList()
    {
        var result = await _ordersService.GetDeliveryOptionList();
        return Ok(result);
    }
    
}