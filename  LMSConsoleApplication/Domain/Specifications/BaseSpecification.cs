using LMSConsoleApplication.Services;

namespace LMSConsoleApplication.Domain.Specifications;

public abstract class BaseSpecification<T>:ISpecification<T>
{
    public BaseSpecification()
    {
        
    }
    
    public BaseSpecification(Func<T,bool> criteria)
    {
        Criteria = criteria;
    }
    
    public Func<T, bool> Criteria { get; }
    
    public Func<T, object> OrderBy { get; private set; }
    
    public Func<T, object> OrderByDescending { get; private set; }
    
    public int Take { get; private set; }
    
    public int Skip { get; private set; }
    
    public bool IsPagingEnabled { get; private set; }


    protected void ApplyPaging(int take, int skip)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
    

    protected void AddOrderBy(Func<T, object> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    
    protected void AddOrderByDescending(Func<T, object> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }
}

public class CourseQueryParams : QueryParams
{
    
}