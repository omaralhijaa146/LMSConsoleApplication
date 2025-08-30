using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class NotNullOrEmptyStringRequirement : IValidRequirement<FullName>
{

    private readonly string _text;
    
    public NotNullOrEmptyStringRequirement(string text)
    {
        _text = text;
        ErrorMessage = "Empty string is not allowed";
    }

    public string ErrorMessage { get; }

    public bool IsMet()
    {
        return !string.IsNullOrWhiteSpace(_text);
    }
    
}