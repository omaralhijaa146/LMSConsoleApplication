namespace LMSConsoleApplication.Utilties;

public static class UserInputReader
{
    public static string ReadUserInput()
        => Console.ReadLine()??"";

    public static string ReadUserInput(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine()??"";
    }
}