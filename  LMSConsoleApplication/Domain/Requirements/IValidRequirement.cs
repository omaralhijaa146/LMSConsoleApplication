namespace LMSConsoleApplication.Utilties;

public interface IValidRequirement<T> where T : class
{
    public bool IsMet();
}