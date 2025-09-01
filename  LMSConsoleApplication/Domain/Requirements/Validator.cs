using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class Validator<T>:IValidator<T> 
{
    private readonly List<Type> _rules = new();
    private readonly List<IValidRequirement<T>> _rulesInitailaized = new();

    public Validator<T> AddRule(IValidRequirement<T> spec)
    {
        _rulesInitailaized.Add(spec);
        return this;
    }
    
    public Validator<T> AddRule<TRequirement>() where TRequirement : IValidRequirement<T>, new()
    {
        _rules.Add(typeof(TRequirement));
        return this;
    }

    public IEnumerable<string> Validate(T entity)
    {

        foreach (var rule in _rulesInitailaized)
        {
            if(!rule.IsMet())
                yield return rule.ErrorMessage;
        }
    }
    
    public IEnumerable<string> ValidateWithInitializingRequirements(T entity)
    {

        foreach (var rule in _rules)
        {
            var initializedRule = (IValidRequirement<T>)Activator.CreateInstance(rule,entity);
            if(initializedRule != null && !initializedRule.IsMet())
                yield return initializedRule.ErrorMessage;
        }
    }
}