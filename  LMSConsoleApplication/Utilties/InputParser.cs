namespace LMSConsoleApplication.Utilties;

public static class InputParser
{
    public static bool TryParseInt(string input, out int result)
    {
        return int.TryParse(input, out result);
    }
    
    public static bool TryParseGuid(string input, out Guid result)
    {
        return Guid.TryParse(input, out result);
    }

    public static bool TryParseDateTime(string input, out DateTime result)
    {
        return DateTime.TryParse(input, out result);
    }
}