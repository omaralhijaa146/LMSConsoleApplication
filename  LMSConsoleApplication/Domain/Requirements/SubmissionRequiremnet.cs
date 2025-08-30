using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class SubmissionRequiremnet : IValidRequirement<Submission>
{
    private readonly Submission _submission;
    
    public SubmissionRequiremnet(Submission submission)
    {
        _submission = submission;
        ErrorMessage = "Wrong reference to assignment id, student id, or submission date ";
    }
    
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return new ValidGuidIdRequirement(_submission.AssignmentId).IsMet() &&
               new ValidGuidIdRequirement(_submission.StudentId).IsMet() && new ValidDateRequirement(_submission.SubmittedAt).IsMet()&& new ScoreRequirement(_submission.Score??0).IsMet()&& new NotNullOrEmptyStringRequirement(_submission.Feedback??"").IsMet();
    }
    
    
}