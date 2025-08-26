namespace LMSConsoleApplication;

public interface IClock
{
    public DateTime Now { get;}
    public DateTime UtcNow { get;}
}