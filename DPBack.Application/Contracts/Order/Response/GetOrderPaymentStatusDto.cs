using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public class GetOrderPaymentStatusDto
{
    public OrderPaymentStatus PaymentStatus { get; set; }
}