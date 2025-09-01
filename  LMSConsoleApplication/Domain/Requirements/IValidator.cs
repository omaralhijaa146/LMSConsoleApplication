namespace LMSConsoleApplication.Domain.Requirements;

public interface IValidator<T>
{
    public Validator<T> AddRule(IValidRequirement<T> spec);
    public IEnumerable<string> ValidateWithInitializingRequirements(T entity);
    public Validator<T> AddRule<TRequirement>() where TRequirement : IValidRequirement<T>, new();
    public IEnumerable<string> Validate(T entity);
}