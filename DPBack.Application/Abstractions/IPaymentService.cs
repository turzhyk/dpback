namespace DPBack.Application.Abstractions;

public interface IPaymentService
{
    Task<string> CreatePayment(string token, decimal totalPrice);
    Task CapturePayment(string orderId);
}