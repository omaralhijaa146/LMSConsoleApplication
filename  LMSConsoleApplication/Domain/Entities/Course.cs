using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Course:Entity
{
    private string _name;
    private string _description;
    
    public string Name { get=>_name;set=>_name=ValidateString(value); }
    public string Description { get=>_description; set=>_description=ValidateString(value); }
    public List<Trainer>? Trainers { get; set; }
    public List<Module>? Modules { get; set; }
    public List<Enrollment>? Enrollments { get; set; }
    public List<Session>? Sessions { get; set; }= new List<Session>();
    public List<Announcement>? Announcements { get; set; }

    
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

    private string ValidateString(string value)
    {
        if(!new NotNullOrEmptyStringRequirement(value).IsMet())
            throw new ArgumentException("Invalid value");
        return value;
    }
    
    public override bool IsValid()
    {
        return new NotNullOrEmptyStringRequirement(Name).IsMet()&&
               new NotNullOrEmptyStringRequirement(Description).IsMet();
    }
}