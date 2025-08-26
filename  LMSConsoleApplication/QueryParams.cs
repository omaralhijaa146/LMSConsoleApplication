namespace LMSConsoleApplication.Services;

public class QueryParams
{
    private const int MaxPageSize = 50;
    private  int _pageSize = 5;
    private string? _search;
    
    public int PageSize { get=>_pageSize; set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
    public int PageNumber { get; set; }
    public string? Search { get=>_search; set => _search = value?.ToLower(); }
}