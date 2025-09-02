using LMSConsoleApplication.Services;

namespace LMSConsoleApplication.Views;

public class PaginationViewHandlers<T> where T : class
{
    public Func<QueryParams, Paging<T>> GetItemsAction;
    public Action<T> PrintItemsAction;
    public string? SearchIndecatorMessage;
}