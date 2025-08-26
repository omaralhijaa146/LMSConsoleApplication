using System.Text.RegularExpressions;

namespace LMSConsoleApplication.Domain.Entities;

public abstract class Person:Entity
{
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    
    protected Person(string firstName,string lastName, string email)
    {
        FullName = new FullName(firstName,lastName);
        Email = new Email(email);
    }
}

