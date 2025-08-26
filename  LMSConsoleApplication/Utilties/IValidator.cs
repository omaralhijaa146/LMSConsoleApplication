namespace LMSConsoleApplication.Utilties;

public interface IValidator<T> where T : class
{
    public bool Validate(IValidRequirement<T> requirement,T entity);
}