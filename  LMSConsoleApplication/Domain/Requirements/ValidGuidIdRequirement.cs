namespace LMSConsoleApplication.Domain.Requirements;

public class ValidGuidIdRequirement:IValidRequirement<Guid>
{
    private readonly Guid _id;

    public ValidGuidIdRequirement(Guid id)
    {
        _id = id;
        ErrorMessage = "Invalid Id Value";
    }
    
    public string ErrorMessage { get; }
    public bool IsMet()
    {
        return Guid.Empty != _id;
    }
}