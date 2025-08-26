using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Utilties;

public class SessionRequirement : IValidRequirement<Session>
{
    private readonly Session _firstSession;
    private readonly Session _secondSession;
    public SessionRequirement(Session firstSession,Session secondSession)
    {
        _firstSession = firstSession;
        _secondSession = secondSession;
    }
    public bool IsMet()
    {
        return _firstSession.TimeRange.IsOverlapping(_secondSession.TimeRange) && _firstSession.Location != _secondSession.Location;
    }
}