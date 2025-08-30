using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class RuleBuilder<T> where T : class
{
    private readonly Func<T,bool> _predicate;
    private readonly string _message;
    private readonly T _entity;
    public RuleBuilder(T entity,Func<T,bool> predicate, string message)
    {
        _predicate = predicate;
        _entity = entity;
        _message = message;
    }

    public IValidRequirement<T> Build() =>
        new CustomRequirement<T>(_entity,_predicate, _message);
}