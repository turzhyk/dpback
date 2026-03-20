using System.Text.Json;
using DPBack.Application.Contracts;
using DPBack.Application.Features;
using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DPBack.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : Controller
{
    private readonly IOrdersService _ordersService;
    private readonly IPaymentService _paymentService;
    private readonly PayUOptions _options;

    public PaymentController(IOptions<PayUOptions> options, IOrdersService ordersService, IPaymentService paymentService)
    {
        _options = options.Value;
       
        _ordersService = ordersService;
        _paymentService = paymentService;
    }

    [HttpPost("notify")]
    public async Task<IActionResult> Notify()
    { 
        // var rawBody = await RawBodyConverter.GetRawBody(Request);
        // Console.WriteLine(rawBody)
        Request.Body.Position = 0;
        using var reader = new StreamReader(Request.Body, leaveOpen: true);
        var rawBody = await reader.ReadToEndAsync();
        Request.Body.Position = 0;
        
        var signatureHeader = Request.Headers["OpenPayu-Signature"];
   
        if(!SignatureVerificator.Verify(rawBody, signatureHeader, _options.SecondKey))
            return Unauthorized();
        
        var dto = JsonSerializer.Deserialize<PayUWebhookDto>(rawBody);
        if (dto == null)
            return BadRequest("invalid notify data");
        
        var orderId = dto.Order.ExtOrderId;
        var payuOrderId = dto.Order.OrderId;
        var status = dto.Order.Status;
        
        switch (status)
        {
            case ("WAITING_FOR_CONFIRMATION"):
                var currentStatus = await _ordersService.GetPaymentStatus(new Guid(orderId));
                if (currentStatus == OrderPaymentStatus.Waiting)
                    await _paymentService.CapturePayment(payuOrderId);
                break;
            case "CANCELED":
                await _ordersService.SetPaymentStatus(new Guid(orderId), OrderPaymentStatus.Cancelled);
                break;
            case "COMPLETED":
                await _ordersService.SetPaymentStatus(new Guid(orderId), OrderPaymentStatus.Paid);
                break;
        }

        return Ok();
    }
}