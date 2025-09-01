using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Announcement:Entity
{
    private Guid _courseId;
    private string _message;
    private DateTime _createdAt;
    
    public Guid CourseId { get=>_courseId; set=>_courseId=ValidateId(value); }
    public string Message { get=>_message; set=>_message=ValidateString(value);}
    public DateTime CreatedAt { get=>_createdAt; set=>_createdAt=ValidateDate(value); }

    
    public Announcement(Guid courseId, string message, DateTime createdAt)
    {
        CourseId = courseId;
        Message = message;
        CreatedAt = createdAt;
        
    }

    private DateTime ValidateDate(DateTime date)
    {
        if(!new ValidDateRequirement(date).IsMet())
            throw new ArgumentException("Invalid date");
        return date;
    }
    private string ValidateString(string value)
    {
        if(!new NotNullOrEmptyStringRequirement(value).IsMet())
            throw new ArgumentException("Invalid value");
        return value;
    }
    

    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(CourseId).IsMet()&&
               new NotNullOrEmptyStringRequirement(Message).IsMet()&&
               new ValidDateRequirement(CreatedAt).IsMet();
    }
}