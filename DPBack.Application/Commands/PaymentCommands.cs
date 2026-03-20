using DPBack.Application.Abstractions;

namespace DPBack.Application.Commands;

public class PaymentCommands
{
    private readonly IPaymentService _paymentService;

    public PaymentCommands(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<string> CreatePayment(string orderId, decimal price)
    {
        return await _paymentService.CreatePayment(orderId,price );
    }
    public async Task CapturePayment(string payuOrderId)
    {
        await _paymentService.CapturePayment(payuOrderId);
    }
}