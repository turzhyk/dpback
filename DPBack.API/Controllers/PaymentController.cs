using System.Text.Json;
using DPBack.Application.Contracts;
using DPBack.Application.Features;
using DPBack.Application.Interfaces;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.Infrastructure.Payments;

[ApiController]
[Route("api/payments")]
public class PaymentController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IOrdersService _ordersService;
    private readonly IPaymentService _paymentService;
    private readonly IPaymentTokenProvider _tokenProvider;

    public PaymentController(IConfiguration configuration, IOrdersService ordersService, IPaymentService paymentService, IPaymentTokenProvider paymentTokenProvider)
    {
        _configuration = configuration;
        _ordersService = ordersService;
        _paymentService = paymentService;
        _tokenProvider = paymentTokenProvider;
    }

    [HttpPost("notify")]
    public async Task<IActionResult> Notify()
    {
        var rawBody = await RawBodyConverter.GetRawBody(Request);

        var signatureHeader = Request.Headers["OpenPayu-Signature"];
        if(!SignatureVerificator.Verify(rawBody, signatureHeader, _configuration["PayU:SecondKey"]))
            return Unauthorized();
        
        var dto = JsonSerializer.Deserialize<PayUWebhookDto>(rawBody);
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