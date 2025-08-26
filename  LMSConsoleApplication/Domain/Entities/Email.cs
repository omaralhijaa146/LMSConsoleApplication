using System.Text.RegularExpressions;
using LMSConsoleApplication.Utilties;

namespace LMSConsoleApplication.Domain.Entities;

public record Email
{
    public string Value { get;}
    public Email(string email)
    {
        Value = email;
    }
    
}