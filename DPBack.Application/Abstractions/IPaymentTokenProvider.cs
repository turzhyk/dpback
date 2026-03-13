namespace DPBack.Application.Abstractions;

public interface IPaymentTokenProvider
{
    Task<string> GetToken();
}