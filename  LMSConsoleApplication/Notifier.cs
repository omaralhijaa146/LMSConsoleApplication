namespace LMSConsoleApplication;

public class Notifier : INotifier
{
    public void WarningNotify(string message)
    {
        ChangeNotifyColor(ConsoleColor.Yellow);
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void ErrorNotify(string message)
    {
        ChangeNotifyColor(ConsoleColor.Red);
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void InfoNotify(string message)
    {
        ChangeNotifyColor(ConsoleColor.White);
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private void ChangeNotifyColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
    }
    
}