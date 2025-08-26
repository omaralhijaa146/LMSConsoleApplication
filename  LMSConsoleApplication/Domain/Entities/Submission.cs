namespace LMSConsoleApplication.Domain.Entities;

public class Submission:Entity
{
    public Guid AssignmentId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime SubmittedAt { get; set; }
    public double? Score { get; set; }
    public string? Feedback { get; set; }
}