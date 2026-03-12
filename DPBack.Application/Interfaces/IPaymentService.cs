namespace DPBack.Application.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePayment(string token);
    Task CapturePayment(string orderId);
}