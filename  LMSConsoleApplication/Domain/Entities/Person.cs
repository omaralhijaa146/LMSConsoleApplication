using System.Text.RegularExpressions;
using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public abstract class Person:Entity
{
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    
    protected Person(string firstName,string lastName, string email)
    {
        var fullName = new FullName(firstName,lastName);
        var userEmail =   new Email(email);
        FullName = fullName;
        Email = userEmail;
        
        if(IsInvalid())
            throw new ArgumentException("Person name or email cannot be empty.");
    }

    
    public override bool IsValid()
    {
        return new NameRequirement(FullName).IsMet()&&new EmailRequirement(Email).IsMet();
    }
}

