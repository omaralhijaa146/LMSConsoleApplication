using System.Text.RegularExpressions;
using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class EmailRequirement : IValidRequirement<Email>
{
    private readonly Regex _emailRegexFormat;
    private readonly Email _email;

    public EmailRequirement(Email email)  
    {
        _email = email;
        _emailRegexFormat= new Regex("^\\S+@\\S+\\.\\S+$");
    }
    public EmailRequirement(string email):this(new Email(email))  
    { }

    public string ErrorMessage { get; }

    public bool IsMet()
    {
        return new NotNullOrEmptyStringRequirement(_email.Value).IsMet() && _emailRegexFormat.IsMatch(_email.Value);
    }
    
}