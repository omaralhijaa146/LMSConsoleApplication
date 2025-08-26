namespace LMSConsoleApplication.Domain.Entities;

public class Session:ScheduledItem
{
    public Guid CourseId { get; }
    public Guid ModuleId { get;  }
    public Guid TrainerId { get;  }
    public string Location { get;  }
    
    public Session(Guid id, Guid courseId, Guid moduleId, Guid trainerId, string location,TimeRange timeRange)
    {
        Id = id;
        CourseId = courseId;
        ModuleId = moduleId;
        TrainerId = trainerId;
        Location = location;
        TimeRange = timeRange;
    }
    
}