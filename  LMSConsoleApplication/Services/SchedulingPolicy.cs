using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Utilties;

namespace LMSConsoleApplication.Services;

public class SchedulingPolicy:ISchedulingPolicy
{
    private readonly LmsContext _lmsContext;
    public SchedulingPolicy(LmsContext lmsContext)
    {
        _lmsContext = lmsContext;
    }

    public bool ValidateOtherSessionSchedule(Session toBeScheduled)
    {
        throw new NotImplementedException();
    }
    
    public bool ValidateModuleAndRoomSchedule(Session toBeScheduled)
    {
        var courseTobeScheduled = _lmsContext.Courses.FirstOrDefault(x => x.Id == toBeScheduled.CourseId);
        
        if (courseTobeScheduled is null)
            return false;
        var module= courseTobeScheduled.Modules.FirstOrDefault(x=>x.Id==toBeScheduled.ModuleId);
        if (module is null)
            return false;
        if (module.Session is null)
        {
            return true;
        }

        var sessionRequirement= new SessionRequirement(module?.Session,toBeScheduled);
        return sessionRequirement.IsMet();
    }
    
    public bool ValidateTrainerSessionSchedule(Session toBeScheduled)
    {
        var courseTobeScheduled = _lmsContext.Courses.FirstOrDefault(x => x.Id == toBeScheduled.CourseId);

        if (courseTobeScheduled is null)
            return false;
        
        var trainer = _lmsContext.Trainers.FirstOrDefault(x => x.Id == toBeScheduled.TrainerId);

        if (trainer is null) 
            return false;
        
        var trainerCourses=  _lmsContext.Courses.Join(trainer.Courses,c=>c.Id,c=>c.Id,(c,t)=>c).ToList();    
          
        if(trainerCourses.FirstOrDefault(x=>x.Id==toBeScheduled.CourseId) is not null)
            return false;

        /*var conflict= trainer.Sessions.FirstOrDefault(x=>x.CourseId==toBeScheduled.CourseId&&!x.IsOverlapping(toBeScheduled.TimeRange));*/
        
        var trainerSession = trainer.Sessions.FirstOrDefault(x => x.CourseId == toBeScheduled.CourseId);
        var trainerSessionRequirement = new SessionRequirement(trainerSession, toBeScheduled);

        if (!trainerSessionRequirement.IsMet())
            return false;

        var trainerHasAvailableTime =
            trainer.AvailabilityWindows.FirstOrDefault(x => toBeScheduled.IsOverlapping(new TimeRange(x.Start,x.End)));
        
        return  trainerSessionRequirement.IsMet()&& trainerHasAvailableTime is not null;
    }
}