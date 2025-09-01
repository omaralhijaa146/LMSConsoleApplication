using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class StudentRequirement:UserRequirement
{

    public StudentRequirement()
    {
        
    }
    
    public StudentRequirement(Student person):base(person)
    {
    }
}