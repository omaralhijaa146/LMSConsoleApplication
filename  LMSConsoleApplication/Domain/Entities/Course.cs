using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Course:Entity
{
    public Course(string name, string description, List<Trainer>? trainers, List<Module>? modules, List<Enrollment>? enrollments, List<Announcement>? announcements,List<Session>?sessions):this(name,description)
    {
        Trainers = trainers;
        Modules = modules;
        Enrollments = enrollments;
        Announcements = announcements;
        Sessions = sessions;
    }

    public Course(){}
    public Course(string name, string description)
    {
        Name = name;
        Description = description;
        if(IsInvalid())
            throw new ArgumentException("Invalid course");
        
        Trainers = new List<Trainer>();
        Modules = new List<Module>();
        Enrollments = new List<Enrollment>();
        Sessions = new List<Session>();
        Announcements = new List<Announcement>();
    }
    
    public string Name { get; }
    public string Description { get; }
    public List<Trainer>? Trainers { get; set; }
    public List<Module>? Modules { get; set; }
    public List<Enrollment>? Enrollments { get; set; }
    public List<Session>? Sessions { get; set; }= new List<Session>();
    public List<Announcement>? Announcements { get; set; }
    public override bool IsValid()
    {
        return new NotNullOrEmptyStringRequirement(Name).IsMet()&&
               new NotNullOrEmptyStringRequirement(Description).IsMet();
    }
}