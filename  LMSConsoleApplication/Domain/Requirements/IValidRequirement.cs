using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public interface IValidRequirement<in T>
{
    public string ErrorMessage { get; }
    public bool IsMet();
}