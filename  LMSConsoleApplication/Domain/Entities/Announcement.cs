namespace LMSConsoleApplication.Domain.Entities;

public class Announcement:Entity
{
    public string CourseId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}