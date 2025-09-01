using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Enrollment:Entity
{
    
    private Guid _courseId;
    private Guid _studentId;
    private DateTime _enrolledAt;
    public Guid CourseId { get=>_courseId; set=>_courseId=ValidateId(value); }
    public Guid StudentId { get=>_studentId; set=>_studentId=ValidateId(value); }
    public DateTime EnrolledAt { get=>_enrolledAt; set=>_enrolledAt=ValidateDate(value); }

    public Enrollment(Guid courseId,Guid studentId,DateTime enrolledAt)
    {
        CourseId = courseId;
        StudentId = studentId;
        EnrolledAt = enrolledAt;
        if(IsInvalid())
            throw new ArgumentException("Invalid enrollment");
    }
    
    private DateTime ValidateDate(DateTime date)
    {
        if(!new ValidDateRequirement(date).IsMet())
            throw new ArgumentException("Invalid date");
        return date;
    }
    
    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(CourseId).IsMet()&&
               new ValidGuidIdRequirement(StudentId).IsMet()&&
               new ValidDateRequirement(EnrolledAt).IsMet();
    }
}