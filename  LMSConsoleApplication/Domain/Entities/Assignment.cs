using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Assignment:Entity
{
    public Guid CourseId { get; }
    public string Title { get;}
    public double MaxScore { get;}
    public DateTime DueAt { get;}
    
    public Assignment(Guid courseId,string title,double maxScore,DateTime dueAt)
    {
        CourseId = courseId;
        Title = title;
        MaxScore = maxScore;
        DueAt = dueAt;
        if(IsInvalid())
            throw new ArgumentException("Invalid assignment");
    }
    
    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(CourseId).IsMet() &&
               new NotNullOrEmptyStringRequirement(Title).IsMet() &&
               new ValidDateRequirement(DueAt).IsMet() &&
               new ScoreRequirement(MaxScore).IsMet();
    }
}