using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IOrdersService _ordersService;

    public PaymentController(IConfiguration configuration, IOrdersService ordersService)
    {
        _configuration = configuration;
        _ordersService = ordersService;
    }

    [HttpPost("notify")]
    public async Task<IActionResult> Notify([FromBody] PayUWebhookDto dto)
    {
        // SHOULD BE VERIFIED
        var raw = System.Text.Json.JsonSerializer.Serialize(dto);
        Console.WriteLine("PAYU WEBHOOK: " + raw);
        var orderId = dto.Order.ExtOrderId;
        var status = dto.Order.Status;
        if (status == "WAITING_FOR_CONFIRMATION")
        {
            await _ordersService.SetPaymentStatus(new Guid(orderId), OrderPaymentStatus.Paid);
        }
        else if (status == "CANCELED")
        {
            await _ordersService.SetPaymentStatus(new Guid(orderId), OrderPaymentStatus.Cancelled);
        }

        return Ok();
    }
}