using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Announcement:Entity
{
    public Announcement(Guid courseId, string message, DateTime createdAt)
    {
        CourseId = courseId;
        Message = message;
        CreatedAt = createdAt;
        if(IsInvalid())
            throw new ArgumentException("Invalid announcement");
    }


    public Guid CourseId { get; }
    public string Message { get;}
    public DateTime CreatedAt { get; }
    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(CourseId).IsMet()&&
               new NotNullOrEmptyStringRequirement(Message).IsMet()&&
               new ValidDateRequirement(CreatedAt).IsMet();
    }
}