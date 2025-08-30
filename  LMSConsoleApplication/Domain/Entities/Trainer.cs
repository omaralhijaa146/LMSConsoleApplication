namespace LMSConsoleApplication.Domain.Entities;

public class Trainer : Person
{
    public Trainer(string firstName, string lastName, string email) : base(firstName,lastName, email)
    {
        if(IsInvalid())
            throw new ArgumentException("Trainer name or email cannot be empty");
        Skills = new List<string>();
        AvailabilityWindows = new List<AvailabilityWindow>();
        Courses = new List<Course>();
        Sessions = new List<Session>();
    }

    public List<string> Skills { get; set; }
    public List<AvailabilityWindow> AvailabilityWindows { get; set; }
    public List<Course>? Courses { get; set; }
    public List<Session>? Sessions { get; set; }
}