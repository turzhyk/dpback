using DPBack.Application.Interfaces;

namespace DPBack.Application.Commands;

public class PaymentCommands
{
    private readonly IPaymentService _paymentService;

    public PaymentCommands(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<string> CreatePayment(string orderId)
    {
        return await _paymentService.CreatePayment(orderId);
    }
    public async Task CapturePayment(string payuOrderId)
    {
        await _paymentService.CapturePayment(payuOrderId);
    }
}