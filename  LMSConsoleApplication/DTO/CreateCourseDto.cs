namespace LMSConsoleApplication.DTO;

public class CreateCourseDto
{

    private string _name;
    public string Name { get=>_name; set=> _name = value.ToLower(); }
    public string Description { get; set; }
}