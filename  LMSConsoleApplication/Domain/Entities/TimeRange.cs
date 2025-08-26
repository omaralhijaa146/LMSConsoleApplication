namespace LMSConsoleApplication.Domain.Entities;

public sealed record TimeRange
{

    public DateTime Start { get; init; }
    public DateTime End { get; init; }

    public TimeRange()
    {
        
    }
    
    public TimeRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }
    
    public TimeSpan Duration => End - Start;
    
    public bool IsOverlapping(TimeRange other)
    {
        return Start < other.End && End > other.Start;
    }
   
}