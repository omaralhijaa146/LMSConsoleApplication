using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Session:ScheduledItem
{
    private Guid _courseId;
    private Guid _moduleId;
    private Guid _trainerId;
    private string _location;
    public Guid CourseId { get=>_courseId; set=>_courseId=ValidateId(value);}
    public Guid ModuleId { get=>_moduleId; set=>_moduleId=ValidateId(value); }
    public Guid TrainerId { get=>_trainerId; set=>_trainerId=ValidateId(value); }
    public string Location { get=>_location; set=>_location=ValidateLocation(value); }
    
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

    private string ValidateLocation(string location)
    {
        if(!new NotNullOrEmptyStringRequirement(location).IsMet())
            throw new ArgumentException("Location cannot be empty");
        return location;
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