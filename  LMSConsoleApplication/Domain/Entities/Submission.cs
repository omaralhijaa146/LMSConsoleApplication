using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Submission:Entity
{
    
    private Guid _assignmentId;
    private Guid _studentId;
    private DateTime _submittedDate;
    private double? _score;
    private string? _feedback;
    public Guid AssignmentId { get=>_assignmentId; set=>_assignmentId=ValidateId(value);}
    public Guid StudentId { get=>_studentId; set=>_studentId=ValidateId(value);}
    public DateTime SubmittedAt { get=>_submittedDate; set=> _submittedDate=ValidateDate(value);}
    public double? Score { get=>_score; set=>_score=ValidateScore(value??0);}
    public string? Feedback { get=>_feedback; set=>_feedback=ValidateFeedback(value);}

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

    private DateTime ValidateDate(DateTime date)
    {
        if (!new ValidDateRequirement(date).IsMet())
            throw new ArgumentException("Invalid date");
        return date;
    }

    private double ValidateScore(double? score)
    {
        if (!new ScoreRequirement(score??0).IsMet())
            throw new ArgumentException("Invalid score");
        return score??0;
    }
    
    private string ValidateFeedback(string feedback)
    {
        if (!new NotNullOrEmptyStringRequirement(feedback).IsMet())
            throw new ArgumentException("Invalid feedback");
        return feedback;
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