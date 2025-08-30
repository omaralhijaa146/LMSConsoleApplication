using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Enrollment:Entity
{
    public Guid CourseId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime EnrolledAt { get; set; }

    public Enrollment(Guid courseId,Guid studentId,DateTime enrolledAt)
    {
        CourseId = courseId;
        StudentId = studentId;
        EnrolledAt = enrolledAt;
        if(IsInvalid())
            throw new ArgumentException("Invalid enrollment");
    }
    
    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(CourseId).IsMet()&&
               new ValidGuidIdRequirement(StudentId).IsMet()&&
               new ValidDateRequirement(EnrolledAt).IsMet();
    }
}