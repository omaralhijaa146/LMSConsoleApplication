using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Requirements;

public class ModuleRequirement:IValidRequirement<Module>
{
    private readonly Module _module;

    public ModuleRequirement()
    {
        ErrorMessage = "Module cannot be null";
    }
    public ModuleRequirement(Module module):this()
    {
        _module = module;
    }
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return new NotNullOrEmptyStringRequirement(_module.Title).IsMet() && _module.Order > 0 &&
               _module.DurationInMinutes > 0 &&
               Enum.IsDefined(typeof(ModuleCompleteStatus), _module.Completed);
    }
}