namespace DPBack.Application.Contracts;

public class CreateOrderResponseDto
{
    public string OrderId { get; set; }
    public string PaymentUrl { get; set; }
}