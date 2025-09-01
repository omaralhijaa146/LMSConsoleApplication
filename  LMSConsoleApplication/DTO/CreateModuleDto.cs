using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.DTO;

public class CreateModuleDto
{
    public ModuleCompleteStatus Completed { get; }
    public string Title { get; set;}

    public int DurationInMinutes { get; set; }
    public int Order { get; set; }
    public bool? Optional { get; set; }
}