namespace DPBack.Domain.Models;

public class OrderHistoryElement
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime ChangedAt { get; set; }
    public string AuthorLogin { get; set; }

    // public OrderHistoryElement(string status, string authorLogin, DateTime date)
    // {
    //     Status = status;
    //     AuthorLogin = authorLogin;
    //     ChangedAt = date;
    // }
}