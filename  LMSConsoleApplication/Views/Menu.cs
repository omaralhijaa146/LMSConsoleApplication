using LMSConsoleApplication.Utilties;

namespace LMSConsoleApplication.Views;

public abstract class Menu
{
    public string Title { get; set; }
    protected Dictionary<int, Delegate> _options;

    public virtual void Display()
    {
        while (true)
        {
            DisplayMenu();
            if (InputParser.TryParseInt(UserInputReader.ReadUserInput(), out int input) && input == 0) break;
            HandleInput(input);
        }
    }

    protected virtual void HandleInput(int input)
    {
        if (_options.TryGetValue(input, out var action))
        {
            var convertedAction =(Action)action;
            convertedAction();
        }
    }

    protected abstract void DisplayMenu();
}