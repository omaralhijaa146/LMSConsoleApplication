namespace LMSConsoleApplication.Domain.Requirements;

public class ScoreRequirement:IValidRequirement<double>
{

    private readonly double _score;

    public ScoreRequirement(double score)
    {
        _score = score;
        ErrorMessage = "Score must be greater than or equal to 0";
    }
    
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return _score >= 0;
    }
}