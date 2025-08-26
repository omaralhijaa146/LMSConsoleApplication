using LMSConsoleApplication.Domain.Enums;

namespace LMSConsoleApplication.Domain.Entities;

public class Student : Person
{
    public Student(string firstName,string lastName, string email,StudentStatus status) : base(firstName,lastName, email)
    {
        Status = status;
        Enrollments = new List<Enrollment>();
    }

    public StudentStatus Status { get; set; }
    public List<Enrollment> Enrollments { get; set; }
    
}