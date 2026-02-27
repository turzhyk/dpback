namespace DPBack.Application.Contracts;

public class OrderHistoryElementResponse
{
    public string Status { get; set; }
    public string AuthorId { get; set; }
    public DateTime ChangedAt { get; set; }
}