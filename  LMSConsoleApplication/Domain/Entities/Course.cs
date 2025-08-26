namespace LMSConsoleApplication.Domain.Entities;

public class Course:Entity
{

    public Course()
    {
        Trainers = new List<Trainer>();
        Modules = new List<Module>();
        Enrollments = new List<Enrollment>();
        Sessions = new List<Session>();
        Announcements = new List<Announcement>();
    }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Trainer>? Trainers { get; set; }
    public List<Module>? Modules { get; set; }
    public List<Enrollment>? Enrollments { get; set; }
    public List<Session>? Sessions { get; set; }= new List<Session>();
    public List<Announcement>? Announcements { get; set; }
}