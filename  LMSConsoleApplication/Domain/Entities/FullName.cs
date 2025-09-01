using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public record FullName
{
    private string _firstName;
    private string _lastName;
    public string FirstName { get=>_firstName; init=> _firstName = ValidateName(value);}
    public string LastName { get=>_lastName;
        init => _lastName = ValidateName(value);
    }
    
    public FullName(string firstName,string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    private string ValidateName(string name)
    {
        if(!new NotNullOrEmptyStringRequirement(name).IsMet())
            throw new ArgumentException("Name cannot be empty");
        return name;
    }

}