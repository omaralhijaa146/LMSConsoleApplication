using System.Text.RegularExpressions;
using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class EmailRequirement : IValidRequirement<Email>
{
    private readonly Regex _emailRegexFormat;
    private readonly Email _email;
    private readonly string _emailString;

    public EmailRequirement()
    {
        ErrorMessage = "Email is not valid";
        _emailRegexFormat= new Regex("^\\S+@\\S+\\.\\S+$");
    }
    public EmailRequirement(Email email) :this() 
    {
        _email = email;
        _emailRegexFormat= new Regex("^\\S+@\\S+\\.\\S+$");
    }
    public EmailRequirement(string email):this()  
    {
        _emailString = email;
    }

    public string ErrorMessage { get; }

    public bool IsMet()
    {
        return new NotNullOrEmptyStringRequirement(_email is null?_emailString:_email.Value).IsMet() && _emailRegexFormat.IsMatch(_email is null?_emailString:_email.Value);
    }
    
}