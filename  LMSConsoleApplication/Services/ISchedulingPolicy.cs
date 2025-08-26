using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Services;

public interface ISchedulingPolicy
{
    public bool ValidateTrainerSessionSchedule(Session toBeScheduled);
    public bool ValidateModuleAndRoomSchedule(Session toBeScheduled);
}