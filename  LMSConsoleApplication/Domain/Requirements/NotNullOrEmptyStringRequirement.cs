using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Utilties;

public class NotNullOrEmptyStringRequirement : IValidRequirement<FullName>
{

    private readonly string _text;
    
    public NotNullOrEmptyStringRequirement(string text)
    {
        _text = text;
    }
    
    public bool IsMet()
    {
        return !string.IsNullOrWhiteSpace(_text);
    }
    
}