using LMSConsoleApplication.DTO;

namespace LMSConsoleApplication.Views;

public class StudentManagementHandlers
{
    public Func<StudentDto, string> CreateStudent;
    public PaginationViewHandlers<StudentDto> PaginationHandlers;
    public PaginationViewHandlers<CourseDto> CoursePaginationHandlers;
    public Func<string,string, (string,string)> EnrollmentAction;
    public Func<string, string, double> ViewStudentProgressInCode;
    public StudentManagementHandlers()
    {
    }
}