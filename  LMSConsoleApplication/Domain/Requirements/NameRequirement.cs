using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Utilties;

public class NameRequirement : IValidRequirement<FullName>
{

    private readonly FullName _fullName;
    
    public NameRequirement(FullName fullName)
    {
        _fullName = fullName;
    }
    
    public bool IsMet()
    {
        return new NotNullOrEmptyStringRequirement(_fullName.FirstName).IsMet()||new NotNullOrEmptyStringRequirement(_fullName.LastName).IsMet();
    }
    
}