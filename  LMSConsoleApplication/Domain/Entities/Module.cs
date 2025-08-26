namespace LMSConsoleApplication.Domain.Entities;

public class Module:Entity
{
    private string _title;
    public Guid Id { get; set; }= Guid.NewGuid();
    
    public ModuleCompleteStatus Completed { get; set; }
    public string Title
    {
        get;
        set;
    }

    public int DurationInMinutes { get; set; }
    public int Order { get; set; }
    public bool? Optional { get; set; }
    public Session? Session { get; set; }
}