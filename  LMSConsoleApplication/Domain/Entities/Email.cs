using System.Text.RegularExpressions;
using LMSConsoleApplication.Domain.Requirements;
using LMSConsoleApplication.Utilties;

namespace LMSConsoleApplication.Domain.Entities;

public record Email
{
    private string _email;
    public string Value { get=>_email;
        init => _email = ValidateEmail(value);
    }
    public Email(string email)
    {
        Value = email;
    }

    private string ValidateEmail(string email)
    {
        if(!new EmailRequirement(email).IsMet())
            throw new ArgumentException("Email cannot be empty.");
        return email;
    }
    
}