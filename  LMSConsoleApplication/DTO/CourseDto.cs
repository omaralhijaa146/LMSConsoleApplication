using LMSConsoleApplication.DTO;

namespace LMSConsoleApplication.Services;

public class CourseDto
{

    private string _name;
    public string? Id { get; set; }
    public string Name { get=>_name; set=> _name = value.ToLower(); }
    public string Description { get; set; }
    public List<TrainerDto> Trainers { get; set; }
}