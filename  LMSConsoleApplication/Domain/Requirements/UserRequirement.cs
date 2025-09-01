using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public abstract class UserRequirement:IValidRequirement<Person>
{
    private readonly Person _person;

    public UserRequirement()
    {
        ErrorMessage = "User Email or Name cannot be null";
    }
    public UserRequirement(Person person):this()
    {
        _person = person;
    }
    
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return new NameRequirement(_person.FullName).IsMet() && new EmailRequirement(_person.Email).IsMet();
    }
}