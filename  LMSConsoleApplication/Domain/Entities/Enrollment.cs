namespace LMSConsoleApplication.Domain.Entities;

public class Enrollment:Entity
{
    public Guid CourseId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime EnrolledAt { get; set; }
}