using LMSConsoleApplication.Domain.Enums;
using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Student : Person
{
    private StudentStatus _status;
    
    public StudentStatus Status { get=>_status; set=> _status= ValidateStatus(value); }
    public List<Enrollment> Enrollments { get; set; }
    
    public Student(string firstName,string lastName, string email,StudentStatus status) : base(firstName,lastName, email)
    {
        if (IsInvalid())
            throw new ArgumentException("Student name or email cannot be empty");
        
        Status = status;
        Enrollments = new List<Enrollment>();
    }

    private StudentStatus ValidateStatus(StudentStatus status)
    {
        if(!Enum.IsDefined(typeof(StudentStatus),status))
            throw new ArgumentException("Invalid status");
        return status;
    }
}