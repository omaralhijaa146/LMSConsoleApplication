namespace LMSConsoleApplication;

public class SystemClock:IClock
{
    public DateTime Now { get => DateTime.Now; }
    public DateTime UtcNow { get => DateTime.UtcNow; }
}