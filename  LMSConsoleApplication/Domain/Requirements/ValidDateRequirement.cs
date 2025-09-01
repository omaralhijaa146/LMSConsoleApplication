namespace LMSConsoleApplication.Domain.Requirements;

public class ValidDateRequirement:IValidRequirement<DateTime>
{
    private readonly DateTime _date;

    public ValidDateRequirement()
    {
        ErrorMessage = "Invalid Date Value";
    }

    public ValidDateRequirement(DateTime date):this()
    {
        _date = date;
    }
    
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return _date != new DateTime();
    }
}