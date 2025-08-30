using LMSConsoleApplication.Domain.Enums;
using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Student : Person
{
    public Student(string firstName,string lastName, string email,StudentStatus status) : base(firstName,lastName, email)
    {
        if (IsInvalid())
            throw new ArgumentException("Student name or email cannot be empty");
        
        Status = status;
        Enrollments = new List<Enrollment>();
    }

    public StudentStatus Status { get; set; }
    public List<Enrollment> Enrollments { get; set; }
    
}