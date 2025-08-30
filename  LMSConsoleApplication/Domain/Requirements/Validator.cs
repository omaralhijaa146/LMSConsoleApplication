using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class Validator<T>:IValidator<T> 
{
    private readonly List<IValidRequirement<T>> _rules = new();

    public Validator<T> AddRule(IValidRequirement<T> spec)
    {
        _rules.Add(spec);
        return this;
    }

    public IEnumerable<string> Validate(T entity)
    {

        foreach (var rule in _rules)
        {
            if(!rule.IsMet())
                yield return rule.ErrorMessage;
        }
    }
}