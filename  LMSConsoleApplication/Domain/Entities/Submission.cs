using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Submission:Entity
{
    public Guid AssignmentId { get;}
    public Guid StudentId { get;}
    public DateTime SubmittedAt { get;}
    public double? Score { get; }
    public string? Feedback { get;}

    Submission(Guid assignmentId, Guid studentId, DateTime submittedAt, double? score, string? feedback)
    {
        AssignmentId = assignmentId;
        StudentId = studentId;
        SubmittedAt = submittedAt;
        Score = score;
        Feedback = feedback;
        if (IsInvalid())
            throw new ArgumentException("Invalid submission");
    }
    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(AssignmentId).IsMet() &&
               new ValidGuidIdRequirement(StudentId).IsMet() &&
               new ValidDateRequirement(SubmittedAt).IsMet()&&
               new ScoreRequirement(Score??0).IsMet()&&
               new NotNullOrEmptyStringRequirement(Feedback??"").IsMet();
    }
}