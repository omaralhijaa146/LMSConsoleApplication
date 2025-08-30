using System.Text.RegularExpressions;
using LMSConsoleApplication.Domain.Requirements;
using LMSConsoleApplication.Utilties;

namespace LMSConsoleApplication.Domain.Entities;

public record Email
{
    public string Value { get;}
    public Email(string email)
    {
        Value = email;
    }

    public bool IsValid()
    {
        return new EmailRequirement(Value).IsMet();
    }
    
}