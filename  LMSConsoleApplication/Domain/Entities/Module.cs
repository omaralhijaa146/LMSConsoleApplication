using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public class Module:Entity
{
    public ModuleCompleteStatus Completed { get; }
    public string Title { get;}

    public int DurationInMinutes { get;}
    public int Order { get; }
    public bool? Optional { get; set; }
    public Session? Session { get; set; }

    
    public Module(Guid id,string title, int durationInMinutes, int order, ModuleCompleteStatus conpleted, bool? optional=null,Session? session=null):this(title,durationInMinutes,order,conpleted,optional,session)
    {
        Id = id;
    }
    public Module(string title, int durationInMinutes, int order, ModuleCompleteStatus conpleted, bool? optional=null,Session? session=null)
    {
        Title = title;
        DurationInMinutes = durationInMinutes;
        Order = order;
        Completed = conpleted;
        Optional = optional;
        Session = session;
        if(IsInvalid())
            throw new ArgumentException("Invalid module");
    }
    
    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(Id).IsMet()&&
            new NotNullOrEmptyStringRequirement(Title).IsMet() && Order >= 0 && DurationInMinutes > 0 &&
               Enum.IsDefined(typeof(ModuleCompleteStatus), Completed);
    }
}