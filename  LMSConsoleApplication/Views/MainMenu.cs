namespace LMSConsoleApplication.Views;

public class MainMenu : Menu
{
    
    public MainMenu(Action studentManagementController,Action trainerManagementController,Action courseManagementController,Action schedulingManagementController)
    {
        Title = "Main Menu";
        _options = new Dictionary<int,Delegate>
        {
            { 1,  studentManagementController},
            { 2, trainerManagementController},
            { 3, courseManagementController },
            { 5, schedulingManagementController }
        };
    }
    

    protected override void DisplayMenu()
    {
        Console.WriteLine("Main Menu: ");
        Console.WriteLine("==========================");
        Console.WriteLine("Please Choose one of the following or press zero to exit: ");
        Console.WriteLine("==========================");
        Console.WriteLine("1. Student Management Center");
        Console.WriteLine("2. Trainer Management Center");
        Console.WriteLine("3. Course Management Center");
        Console.WriteLine("2. Session Scheduling Center");
        Console.WriteLine("==========================");
       
    }
}