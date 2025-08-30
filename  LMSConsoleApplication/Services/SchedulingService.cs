using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.DTO;

namespace LMSConsoleApplication.Services;

public class SchedulingService
{
    private readonly LmsContext _lmsContext;
    private readonly IEventBuss _eventBuss;
    private readonly IClock _clock;
    private readonly ISchedulingPolicy _policy;

    public SchedulingService(LmsContext lmsContext,IEventBuss eventBuss,IClock clock)
    {
        _lmsContext = lmsContext;
        _eventBuss = eventBuss;
        _clock = clock;
        _policy = new SchedulingPolicy(_lmsContext);
    }

    public string CreateSession(SessionDto sessionDto)
    {
        var session = new Session( Guid.Parse(sessionDto.CourseId),Guid.Parse(sessionDto.ModuleId),Guid.Parse(sessionDto.TrainerId),sessionDto.Location,sessionDto.TimeRange );

        if (!_policy.ValidateModuleAndRoomSchedule(session)&&!_policy.ValidateTrainerSessionSchedule(session))
            throw new InvalidOperationException("Session schedule is not valid");

        var course = _lmsContext.Courses.FirstOrDefault(x => x.Id == Guid.Parse(sessionDto.CourseId));
        _lmsContext.Sessions.Add(session);
        course?.Sessions.Add(session);
        course.Modules.FirstOrDefault(x => x.Id == Guid.Parse(sessionDto.ModuleId)).Session = session;
        _lmsContext.Trainers.FirstOrDefault(x=>x.Id==Guid.Parse(sessionDto.TrainerId))?.Sessions.Add(session);
        return session.Id.ToString();
    }
    
    public void CancelSession(SessionDto sessionDto)
    {
        
        var session = _lmsContext.Sessions.FirstOrDefault(s => s.Id == Guid.Parse(sessionDto.Id));
        if (session == null)
            throw new ArgumentException("Session not found");
        
        var associatedModule = _lmsContext.Courses.FirstOrDefault(x=>x.Id==Guid.Parse(sessionDto.Id)&& x?.Enrollments?.Count>0)?.Modules?.FirstOrDefault(m => m.Session?.Id == Guid.Parse(sessionDto.Id));
        if (associatedModule != null)
        {
            associatedModule.Session = null; // Clear session reference from the module
        }
        
        _lmsContext.Sessions.Remove(session);
        var trainerSessions = _lmsContext.Trainers.FirstOrDefault(x => x.Id == Guid.Parse(sessionDto.TrainerId))
            ?.Sessions;
        var coursesSessions =
            _lmsContext.Courses.FirstOrDefault(x => x.Id == Guid.Parse(sessionDto.CourseId))?.Sessions;
        
        var courseSessionIndex=coursesSessions?.FindIndex(x => x.Id == Guid.Parse(sessionDto.Id));
        var trainerSessionIndex=trainerSessions?.FindIndex(x=>x.Id==Guid.Parse(sessionDto.Id));
        if(trainerSessionIndex.HasValue) trainerSessions?.RemoveAt(trainerSessionIndex.Value);
        if(courseSessionIndex.HasValue) coursesSessions?.RemoveAt(courseSessionIndex.Value);
    }   
    
}

/*
 * Sessions (Scheduling)

Create/List sessions for a course & module with a chosen trainer

Detect conflicts: same trainer overlapping times; same room/time clash (if you model rooms)

Reschedule/cancel sessions
 */
 
