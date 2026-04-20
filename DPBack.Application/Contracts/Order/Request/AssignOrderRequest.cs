namespace DPBack.Application.Contracts;


public record AssignOrderRequest(Guid OrderId, string AuthorLogin);
