using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Utilties;

public class GlobalValidator
{
    private static readonly Dictionary<Type, object> _validators = new();

    // Register a validator for a specific type
    public void Register<T>(Validator<T> validator) where T : class
    {
        _validators[typeof(T)] = validator;
    }

    // Validate any entity
    public void Validate<T>(T entity) where T : class
    {
        if (_validators.TryGetValue(typeof(T), out var validatorObj))
        {
            var validator = (Validator<T>)validatorObj;
            validator.Validate(entity);
        }
        else
        {
            throw new InvalidOperationException($"No validator registered for {typeof(T).Name}");
        }
    }
}