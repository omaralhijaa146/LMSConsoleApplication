using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class SessionRequirement : IValidRequirement<Session>
{
    private readonly Session _firstSession;
    private readonly Session _secondSession;
    public SessionRequirement(Session firstSession,Session secondSession)
    {
        _firstSession = firstSession;
        _secondSession = secondSession;
        ErrorMessage = "Sessions are overlapping";
    }

    public string ErrorMessage { get; }

    public bool IsMet()
    {
        return _firstSession.TimeRange.IsOverlapping(_secondSession.TimeRange) && _firstSession.Location != _secondSession.Location;
    }
}