using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Assignment:Entity
{
    private Guid _courseId;
    private string _title;
    private double _maxScore;
    private DateTime _dueAt;
    
    public Guid CourseId { get=>_courseId; set=>_courseId=ValidateId(value); }
    public string Title { get=>_title; set=>_title=ValidateString(value);}
    public double MaxScore { get=>_maxScore; set=>_maxScore=ValidateScore(value);}
    public DateTime DueAt { get=>_dueAt; set=>_dueAt=ValidateDate(value);}
    
    public Assignment(Guid courseId,string title,double maxScore,DateTime dueAt)
    {
        CourseId = courseId;
        Title = title;
        MaxScore = maxScore;
        DueAt = dueAt;
        if(IsInvalid())
            throw new ArgumentException("Invalid assignment");
    }
    
    private double ValidateScore(double score)
    {
        if(!new ScoreRequirement(score).IsMet())
            throw new ArgumentException("Invalid score");
        return score;
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
        return new ValidGuidIdRequirement(CourseId).IsMet() &&
               new NotNullOrEmptyStringRequirement(Title).IsMet() &&
               new ValidDateRequirement(DueAt).IsMet() &&
               new ScoreRequirement(MaxScore).IsMet();
    }
}