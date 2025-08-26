namespace LMSConsoleApplication.Domain.Entities;

public abstract class ScheduledItem:Entity
{
    private TimeRange _timeRange;
    public TimeRange TimeRange
    { get=>_timeRange;
        set => CheckAndSetTimeRange(value);}
    
    public virtual bool IsOverlapping(TimeRange timeRange)
    {
        if (_timeRange is null)
            return false;
        return TimeRange.Start < timeRange.End && TimeRange.End > timeRange.Start;
    }

    private void CheckAndSetTimeRange(TimeRange timeRange)
    {
        if(IsOverlapping(timeRange))
            throw new ArgumentException("Time range is overlapping");
        _timeRange = timeRange;
    }
}