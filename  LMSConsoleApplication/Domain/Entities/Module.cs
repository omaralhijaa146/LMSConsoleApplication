using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;







public class Module:Entity
{
    private string _title;
    private int _order;
    private int _duartionInMinutes;
    public ModuleCompleteStatus Completed { get; }
    public string Title { get=>_title; set=>_title=ValidateTitle(value);}

    public int DurationInMinutes { get=>_duartionInMinutes; set=>_duartionInMinutes=ValidateGreaterThanZero(value); }
    public int Order { get=>_order; set=>_order=ValidateGreaterThanZero(value); }
    public bool? Optional { get; set; }
    public Session? Session { get; set; }

    
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

    private string ValidateTitle(string title)
    {
        if(!new NotNullOrEmptyStringRequirement(title).IsMet())
            throw new ArgumentException("Title cannot be empty");
        return title;
    }
    
    private int ValidateGreaterThanZero(int value)
    {
        if(value<0)
            throw new ArgumentException("Duration cannot be less than or equal to zero");
        return value;
    }
    public override bool IsValid()
    {
        return new ValidGuidIdRequirement(Id).IsMet()&&
            new NotNullOrEmptyStringRequirement(Title).IsMet() && Order > 0 && DurationInMinutes > 0 &&
               Enum.IsDefined(typeof(ModuleCompleteStatus), Completed);
    }
}