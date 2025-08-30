using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class CustomRequirement<T> : IValidRequirement<T> where T : class
{
    private readonly Func<T,bool> _predicate;
    private readonly T _entity;
    public string ErrorMessage { get; }

    public CustomRequirement(T entity,Func<T,bool> predicate, string errorMessage)
    {
        _predicate = predicate;
        ErrorMessage = errorMessage;
        _entity = entity;
    }
    
    public bool IsMet()=> _predicate(_entity);
}