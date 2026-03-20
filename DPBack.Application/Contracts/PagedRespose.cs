namespace DPBack.Application.Contracts;

public class PagedRespose<T> where T : class
{
    public List<T>? Items { get; set; }
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}