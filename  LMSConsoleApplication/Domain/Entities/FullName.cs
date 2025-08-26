namespace LMSConsoleApplication.Domain.Entities;

public record FullName
{
    public string FirstName { get;}
    public string LastName { get;}
    
    public FullName(string firstName,string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    
}