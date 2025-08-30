using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Session:ScheduledItem
{
    public Guid CourseId { get; }
    public Guid ModuleId { get;  }
    public Guid TrainerId { get;  }
    public string Location { get;  }
    
    public Session( Guid courseId, Guid moduleId, Guid trainerId, string location,TimeRange timeRange)
    {
        CourseId = courseId;
        ModuleId = moduleId;
        TrainerId = trainerId;
        Location = location;
        TimeRange = timeRange;
        if(IsInvalid())
            throw new ArgumentException("Invalid session");
    }

    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(CourseId).IsMet()&&
               new ValidGuidIdRequirement(ModuleId).IsMet()&&
               new ValidGuidIdRequirement(TrainerId).IsMet()&&
               new NotNullOrEmptyStringRequirement(Location).IsMet()&&
               new ValidDateRequirement(TimeRange.Start).IsMet()&&
               new ValidDateRequirement(TimeRange.End).IsMet();
    }
}