using System.ComponentModel.DataAnnotations;

namespace DPBack.Application.Options;

public class PayUOptions
{
    public string Secret { get; set; }
    public string SecondKey { get; set; }
    public string ClientId { get; set; }
    public string NotifyUrl { get; set; }
}