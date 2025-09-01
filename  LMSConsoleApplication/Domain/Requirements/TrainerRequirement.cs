using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class TrainerRequirement:UserRequirement
{
    public TrainerRequirement()
    {
    }

    public TrainerRequirement(Trainer person):base(person)
    {
    }
}