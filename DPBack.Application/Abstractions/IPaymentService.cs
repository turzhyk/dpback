namespace DPBack.Application.Abstractions;

public interface IPaymentService
{
    Task<string> CreatePayment(string token);
    Task CapturePayment(string orderId);
}