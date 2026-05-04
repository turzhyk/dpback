namespace DPBack.Infrastructure.Exceptions;

public class PayUException:Exception
{
    public PayUException(string text) : base(text)
    {}
}