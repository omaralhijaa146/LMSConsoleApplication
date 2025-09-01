using System.Text.RegularExpressions;
using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public abstract class Person:Entity
{
    private FullName _fullName;
    private Email _email;
    public FullName FullName { get=>_fullName; set=> _fullName=ValidateFullName(value); }
    public Email Email { get=>_email;  set=>_email=ValidateEmail(value); }

    protected Person(string firstName, string lastName, string email)
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
    private FullName ValidateFullName(FullName fullName)
    {
        if(!new NameRequirement(fullName).IsMet())
            throw new ArgumentException("Person name cannot be empty.");
        return fullName;
    }
    private Email ValidateEmail(Email email)
    {
        if(!new EmailRequirement(email).IsMet())
            throw new ArgumentException("Person email cannot be empty.");
        return email;
    }
}

