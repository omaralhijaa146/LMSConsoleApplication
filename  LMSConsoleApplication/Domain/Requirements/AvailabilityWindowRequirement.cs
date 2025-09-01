using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class AvailabilityWindowRequirement:IValidRequirement<AvailabilityWindow>
{
    private readonly AvailabilityWindow _availabilityWindow;

    public AvailabilityWindowRequirement()
    {
        ErrorMessage = "Availability Window dates should have proper format";
    }

    public AvailabilityWindowRequirement(AvailabilityWindow availabilityWindow):this()
    {
        _availabilityWindow = availabilityWindow;
    }
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return new ValidDateRequirement(_availabilityWindow.Start).IsMet()&&new ValidDateRequirement(_availabilityWindow.End).IsMet();
    }
}