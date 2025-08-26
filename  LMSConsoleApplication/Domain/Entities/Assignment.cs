namespace LMSConsoleApplication.Domain.Entities;

public class Assignment:Entity
{
    public Guid CourseId { get; set; }
    public string Title { get; set; }
    public double MaxScore { get; set; }
    public DateTime DueAt { get; set; }
}