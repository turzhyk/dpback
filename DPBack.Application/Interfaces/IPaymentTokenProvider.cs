namespace DPBack.Application.Interfaces;

public interface IPaymentTokenProvider
{
    Task<string> GetToken();
}