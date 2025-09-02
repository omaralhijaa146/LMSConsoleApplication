using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Enums;
using LMSConsoleApplication.Domain.Requirements;
using LMSConsoleApplication.DTO;
using LMSConsoleApplication.Services;
using LMSConsoleApplication.Utilties;

namespace LMSConsoleApplication.Views;

public class StudentManagementMenu : Menu
{
    private readonly StudentManagementHandlers _handlers;

    public StudentManagementMenu(StudentManagementHandlers handlers)
    {
        _handlers = handlers;
        Title = "Student Management";
        _options = new Dictionary<int, Delegate>
        {
            { 1,  CreateStudent },
            { 2, ListStudents },
            { 3, SearchStudentByEmail },
            { 4, EnrollStudentInCourse },
            { 5, ViewStudentProgressInCode },
        };
    }
    
    protected override void DisplayMenu()
    {
        Console.WriteLine("Student Management Center: ");
        Console.WriteLine("==========================");
        Console.WriteLine("Plaese Choose one of the following: ");
        Console.WriteLine("==========================");
        Console.WriteLine("1. Create Student");
        Console.WriteLine("2. List Students");
        Console.WriteLine("3. Search Student By Email");
        Console.WriteLine("4. Enroll Student In Course");
        Console.WriteLine("5. View Student Progress In Code");
        Console.WriteLine("0. Exit");
        Console.WriteLine("==========================");
    }


    public void CreateStudent()
    {
        Console.WriteLine("Create Student");
        var studentEmail= UserInputReader.ReadUserInput("Enter Student Email");
        var studentFirstName= UserInputReader.ReadUserInput("Enter Student First Name");
        var studentLastName= UserInputReader.ReadUserInput("Enter Student Last Name");
        var studentDto= new StudentDto
        {
            Email = new Email(studentEmail),
            FullName = new FullName(studentFirstName,studentLastName),
            Status = StudentStatus.Active
        };
        
        _handlers.CreateStudent(studentDto);
    }

    public void ListStudents()
    {
        var pagingView = new PaginationView<StudentDto>(_handlers.PaginationHandlers,"List Students");
        pagingView.ListItems();
    }
    
    public void SearchStudentByEmail()
    {
        
        var pagingView = new PaginationView<StudentDto>(_handlers.PaginationHandlers,"Search Student By Email");
        pagingView.SearchItems();
    }

    public void EnrollStudentInCourse()
    {
        Console.WriteLine("Enroll Student In Course");
        Console.WriteLine("Search or choose the student you wish to enroll by copying and pasting the student id:");

        var stdsPagingView = new PaginationView<StudentDto>(_handlers.PaginationHandlers,"Search Student By Email");
        stdsPagingView.SearchItems();
        var studentId= UserInputReader.ReadUserInput("Enter Student Id: ");
        Console.WriteLine("Search or choose the course you wish to enroll the student in by copying and pasting the course id:");
        var coursesPagingView = new PaginationView<CourseDto>(_handlers.CoursePaginationHandlers,"Courses List");
        coursesPagingView.SearchItems();
        
        var courseId= UserInputReader.ReadUserInput("Enter Course Id: ");
        var enrolledStudentId= _handlers.EnrollmentAction(studentId,courseId);
        Console.WriteLine($"Student with the id {enrolledStudentId.Item1} Enrolled in Course with the id {enrolledStudentId.Item2}");
    }
    
    
    public void ViewStudentProgressInCode()
    {
        Console.WriteLine("Search or choose the student you wish to see his progress by copying and pasting the student id:");

        var stdsPagingView = new PaginationView<StudentDto>(_handlers.PaginationHandlers,"Search Student By Email");
        stdsPagingView.SearchItems();
        
        var studentId= UserInputReader.ReadUserInput("Enter Student Id: ");
        Console.WriteLine($"Search or choose the course you wish to see the student with the id {studentId} progress in by copying and pasting the course id:");
        var coursesPagingView = new PaginationView<CourseDto>(_handlers.CoursePaginationHandlers,"Courses List");
        coursesPagingView.SearchItems();
        
        var courseId= UserInputReader.ReadUserInput("Enter Course Id: ");
        var progress=_handlers.ViewStudentProgressInCode(studentId,courseId);
        Console.WriteLine($"Student with the id {studentId} has {Math.Round(progress,2)} % progress in the course with the id {courseId}");
    }
}