namespace LMSConsoleApplication.Domain.Requirements;

public class ValidDateRequirement:IValidRequirement<DateTime>
{
    private readonly DateTime _date;

    public ValidDateRequirement(DateTime date)
    {
        _date = date;
        ErrorMessage = "Invalid Date Value";
    }
    
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return _date != new DateTime();
    }
}