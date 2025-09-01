using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class CreateCourseRequirement:IValidRequirement<Course>
{
    private readonly string _courseName;
    private readonly string _courseDescription;
    private readonly Course _course;

    public CreateCourseRequirement()
    {
        ErrorMessage = "Name and Description cannot be empty";
    }
    public CreateCourseRequirement(Course course):this()
    {
        _course = course;
    }
    
    public CreateCourseRequirement(string courseName,string courseDescription):this()
    {
        _courseName = courseName;
        _courseDescription = courseDescription;
    }
    
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return new NotNullOrEmptyStringRequirement(_course is null?_courseName:_course.Name).IsMet() &&
               new NotNullOrEmptyStringRequirement(_course is null?_courseDescription:_course.Description).IsMet();
    }
}