namespace LMSConsoleApplication.Domain.Requirements;

public interface IValidator<T>
{
    public Validator<T> AddRule(IValidRequirement<T> spec);
    public IEnumerable<string> Validate(T entity);
}