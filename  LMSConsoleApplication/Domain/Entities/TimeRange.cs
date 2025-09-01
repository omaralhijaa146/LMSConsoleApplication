using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public sealed record TimeRange
{
    
    private DateTime _start;
    private DateTime _end;

    public DateTime Start { get=>_start; init=>_start = ValidateDate(value); }
    public DateTime End { get=>_end; init=>_end = ValidateDate(value); }
    
    public TimeRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    private DateTime ValidateDate(DateTime date)
    {
        if(!new ValidDateRequirement(date).IsMet())
            throw new ArgumentException("Invalid date");
        return date;
    }

    public TimeSpan Duration => End - Start;
    
    public bool IsOverlapping(TimeRange other)
    {
        return Start < other.End && End > other.Start;
    }
   
}