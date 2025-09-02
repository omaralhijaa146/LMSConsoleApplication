using LMSConsoleApplication.Services;
using LMSConsoleApplication.Utilties;

namespace LMSConsoleApplication.Views;

public class PaginationView<T>  : Menu where T : class
{
    private readonly PaginationViewHandlers<T> _handlers;

    public PaginationView(PaginationViewHandlers<T> handlers,string title = "Pagination View")
    {
        _handlers = handlers;
        Title = title;
        _options = new Dictionary<int, Delegate>
        {
            {1,ListItems}, {2,SearchItems}
        };
        
    }

    public void ListItems()
    {
        var isExit= false;
        while (!isExit)
        {
            var isValidPageSize=InputParser.TryParseInt(UserInputReader.ReadUserInput("Enter how many items you wish to have: "), out int itemsPerPage);
            if (!isValidPageSize)
            {
                Console.WriteLine("Invalid Number"); 
                Console.WriteLine("Enter a valid Number");
            }
            
            if(!TryParseUserPageSizeInput(itemsPerPage,out var queryParams))
                continue;
           
            isExit= ListItemsOperation(_handlers.GetItemsAction,_handlers.PrintItemsAction,queryParams,false);;
        }
    }

    public void SearchItems()
    {
        var isExit= false;
        while (!isExit)
        {
            var isValidPageSize=InputParser.TryParseInt(UserInputReader.ReadUserInput("Enter how many items you wish to have: "), out int itemsPerPage);
            if (!isValidPageSize)
            {
                Console.WriteLine("Invalid Number"); 
                Console.WriteLine("Enter a valid Number");
            }
            
            if(!TryParseUserPageSizeInput(itemsPerPage,out var queryParams))
                continue;
            var whatToBeSearchedBy= UserInputReader.ReadUserInput(_handlers.SearchIndecatorMessage??"");
            if (string.IsNullOrWhiteSpace(whatToBeSearchedBy))
            {
                Console.WriteLine("Invalid Input");
                Console.WriteLine("Enter a valid Input");
                continue;
            }
            queryParams.Search= whatToBeSearchedBy;
            isExit= ListItemsOperation(_handlers.GetItemsAction,_handlers.PrintItemsAction,queryParams,true);;
        }
    }

    private bool TryParseUserPageSizeInput(int pageSize,out QueryParams validatedQueryParams)
    {
        
        var queryParams= new QueryParams();
        queryParams.PageNumber = 1;
        queryParams.PageSize = pageSize;
        if (queryParams.PageSize <= 0)
        {
            validatedQueryParams = new QueryParams();
            return false;
        }
        validatedQueryParams= queryParams;
        return true;
    }

    private bool ListItemsOperation(Func<QueryParams,Paging<T>> getItemsAction,Action<T> printItemsAction, QueryParams queryParams ,bool isSearch=false)
    {
        var newQueryParam = new QueryParams { PageNumber = queryParams.PageNumber, PageSize = queryParams.PageSize, Search = isSearch?queryParams.Search:"" }; 
        
        var isExit= false;
        while (!isExit)
        {
            
            var result = getItemsAction(newQueryParam);
            
            Console.WriteLine(result.TotalItems); 
            
            Console.WriteLine(result.PageNumber); 
            
            Console.WriteLine(result.PageSize);

            foreach (var item in result.Result)
            {
                printItemsAction(item);
            } 
            
            Console.WriteLine("Press 0 to exit or press any other number to continue");
            
            var userInput= UserInputReader.ReadUserInput();

            if (!InputParser.TryParseInt(userInput, out int input))
            {
                Console.WriteLine("Invalid input"); Console.WriteLine("Enter a valid input"); continue;
            }

            switch (input)
            {
                case 0: 
                    isExit = true;
                   break;
                default:
                    newQueryParam.PageNumber++;
                    break;
            }
        }

        return true;
    }



    protected override void DisplayMenu()
    {
        Console.WriteLine(Title);
        Console.WriteLine("==========================");
        Console.WriteLine("Please Choose one of the following: ");
        Console.WriteLine("==========================");
        Console.WriteLine("1. List Items");
        Console.WriteLine("2. Search Items");
        Console.WriteLine("==========================");
        Console.WriteLine("Enter your choice: ");
    }
}