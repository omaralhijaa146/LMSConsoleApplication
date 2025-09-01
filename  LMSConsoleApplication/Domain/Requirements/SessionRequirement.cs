using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class SessionRequirement : IValidRequirement<Session>
{
    private readonly Session _firstSession;
    private readonly Session _secondSession;

    public SessionRequirement()
    {
        ErrorMessage = "Sessions are overlapping";
    }
    public SessionRequirement(Session firstSession,Session secondSession):this()
    {
        _firstSession = firstSession;
        _secondSession = secondSession;
    }

    public string ErrorMessage { get; }

    public bool IsMet()
    {
        return _firstSession.TimeRange.IsOverlapping(_secondSession.TimeRange) && _firstSession.Location != _secondSession.Location;
    }
}