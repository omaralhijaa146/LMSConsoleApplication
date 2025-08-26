
namespace LMSConsoleApplication.Domain.Entities;

public record AvailabilityWindow
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
}