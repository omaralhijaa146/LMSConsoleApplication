namespace LMSConsoleApplication.Services;

public class Paging<T> where T:class
{
    public Paging(int pageNumber,int pageSize, int totalItems, IReadOnlyList<T> result)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        Result = result;
    }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public IReadOnlyList<T>? Result { get; set; }
}