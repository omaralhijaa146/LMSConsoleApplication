using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Data;

public class LmsContext
{
    public LmsContext()
    {
        Students = new List<Student>();
        Trainers = new List<Trainer>();
        Courses = new List<Course>(){new Course("csharp","C# is a programming language")};
        Sessions = new List<Session>();
        Enrollments = new List<Enrollment>();
        Assignments = new List<Assignment>();
        Submissions = new List<Submission>();
        Announcements = new List<Announcement>();
    }

    public List<Student> Students { get; set; }

    public List<Trainer> Trainers { get; set; }

    public List<Course> Courses { get; set; }

    public List<Session> Sessions { get; set; }

    public List<Enrollment> Enrollments { get; set; }

    public List<Assignment> Assignments { get; set; }

    public List<Submission> Submissions { get; set; }

    public List<Announcement> Announcements { get; set; }
    
}