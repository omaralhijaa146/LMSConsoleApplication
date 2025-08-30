using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class NameRequirement : IValidRequirement<FullName>
{

    private readonly FullName _fullName;
    
    public NameRequirement(FullName fullName)
    {
        _fullName = fullName;
    }

    public string ErrorMessage { get; }

    public bool IsMet()
    {
        return new NotNullOrEmptyStringRequirement(_fullName.FirstName).IsMet()||new NotNullOrEmptyStringRequirement(_fullName.LastName).IsMet();
    }
    
}