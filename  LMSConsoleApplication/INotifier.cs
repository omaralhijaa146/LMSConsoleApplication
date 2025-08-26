namespace LMSConsoleApplication;

public interface INotifier
{
    public void WarningNotify(string message);   
    public void ErrorNotify(string message);   
    public void InfoNotify(string message);   
}