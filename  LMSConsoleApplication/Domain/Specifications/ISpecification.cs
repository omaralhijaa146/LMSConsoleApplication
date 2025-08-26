namespace LMSConsoleApplication.Domain.Specifications;

public interface ISpecification<T>
{
    Func<T,bool> Criteria { get; }
    
    
    Func<T,object> OrderBy { get; }
    
    Func<T,object> OrderByDescending { get; }
    
    int Take { get; }
    int Skip { get; }
    
    bool IsPagingEnabled { get;  }
}