using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.DTO;

public class SessionDto
{
    public string Id { get; init; }
    public string CourseId { get; init; }
    public string ModuleId { get; init; }
    public string TrainerId { get; init; }
    public string Location { get; init; }
    
    public TimeRange TimeRange { get; init; }
}