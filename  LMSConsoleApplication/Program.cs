using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Enums;
using LMSConsoleApplication.Domain.Events;
using LMSConsoleApplication.Domain.Requirements;
using LMSConsoleApplication.DTO;
using LMSConsoleApplication.Helpers;
using LMSConsoleApplication.Services;

namespace LMSConsoleApplication;

class Program
{
    static void Main(string[] args)
    {
        var eventBus = new EventBuss();
        var notifier = new Notifier();
        var clock = new SystemClock();
        var courseValidator = new Validator<Course>().AddRule<CreateCourseRequirement>();
        var moduleValidator = new Validator<Module>().AddRule<ModuleRequirement>();
        var availabilityWindowValidator = new Validator<AvailabilityWindow>().AddRule<AvailabilityWindowRequirement>();
        var trainerValidator = new Validator<Trainer>().AddRule<TrainerRequirement>();
        var studentValidator = new Validator<Student>().AddRule<StudentRequirement>();
        var sessionValidator = new Validator<Session>().AddRule<SessionRequirement>();
        eventBus.Subscribe<TrainerAssignedToCourse>(x=>notifier.InfoNotify($"Trainer {x.TrainerId} assigned to course {x.CourseId}"));
        eventBus.Subscribe<StudentEnrolled>(x=>notifier.InfoNotify($"Trainer {x.CourseId} assigned to course {x.CourseId}"));
        eventBus.Subscribe<SessionScheduled>(x=>notifier.InfoNotify($"Session Scheduled {x.sessionDto.CourseId}  {x.sessionDto.Id} {x.sessionDto.ModuleId} {x.sessionDto.TrainerId}"));
        
        var context = new LmsContext();
        var courseService = new CourseService(context,eventBus,clock,courseValidator,moduleValidator);
        var trainerService = new TrainerService(context,eventBus,clock,availabilityWindowValidator,trainerValidator);
        trainerService.CreateTrainer(new TrainerDto
        {
            FullName = new FullName("omar","kamal"),
            Email = new Email("omar@omar.com"),
        });
       
        var courseId= courseService.CreateCourse(new CreateCourseDto{Name = "C#",Description = "C# course"});
        var course = courseService.SearchCourses(new QueryParams
        { 
            Search = "C#"
        }).Result.FirstOrDefault();
        
        var trainer=trainerService.SearchTrainers(new QueryParams{Search = "omar"}).Result.FirstOrDefault();
        // Console.WriteLine($"{trainer.Result[0].Id} {trainer.Result[0].FullName.FirstName} {trainer.Result[0].FullName.LastName}");
        courseService.AssignTrainer(courseId,trainer.Id);
        var courses = courseService.ListCourses(new QueryParams
        {
            PageNumber = 1,
            PageSize = 2
        });;
        Console.WriteLine(courses.TotalItems);
        Console.WriteLine(courses.PageNumber);
        Console.WriteLine(courses.PageSize);
        foreach (var courseDto in courses.Result)
        {
            Console.WriteLine(courseDto.Name);
        }
        
        foreach (var courseDto in courseService.SearchCourses(new QueryParams
                 {
                     Search = "C#"
                 }).Result!)
        {
           Console.WriteLine(courseDto.Name); 
           Console.WriteLine(courseDto.Id); 
           courseDto.Trainers.ForEach(x=>Console.WriteLine(x.FullName.FirstName));
        }

        var moduleId = courseService.AddModule(courseId,new CreateModuleDto()
        {
            Optional = true,
            Order = 1,
            DurationInMinutes = 20,
            Title = "Module 1",
        });

        var studentEnrollmentService = new StudentEnrollmentService(context,eventBus,clock,studentValidator);
        var stdId=studentEnrollmentService.CreateStudent(new StudentDto
        {
            Email = new Email("omar@uu.com"),
            FullName = new FullName("omar","kamal"),
            Status = StudentStatus.Active
        });
       var (studentId,courseIdStd)= studentEnrollmentService.EnrollStudentInCourse(stdId,courseId);
       var result= studentEnrollmentService.ListStudents(new QueryParams()
       {
           PageNumber = 1,
           PageSize = 2,
           Search = "kamal"
       });
       Console.WriteLine(result.Result.FirstOrDefault().FullName);
       var sessionService = new SchedulingService(context,eventBus,clock,sessionValidator);;
       var sessionId= sessionService.CreateSession(new SessionDto
        {
            CourseId = courseId,
            ModuleId = moduleId.ToString(),
            TrainerId = trainer.Id,
            Location = "1",
            TimeRange = new TimeRange(DateTime.Now,DateTime.Now.AddHours(1))
        });
       
        // var sessionId2= sessionService.CreateSession(new SessionDto
        // {
        //     CourseId = courseId,
        //     ModuleId = moduleId.ToString(),
        //     TrainerId = trainer.Id,
        //     Location = "1",
        //     TimeRange = new TimeRange(DateTime.Now.AddMinutes(60),DateTime.Now.AddMinutes(-20))
        // });
       context.Courses.FirstOrDefault(x=>x.Id==Guid.Parse(courseId)).Sessions.ForEach(x=>Console.WriteLine(x.TimeRange.Start));;
       
    }
}
public static class UserInputReader
{
    public static string ReadUserInput()
        => Console.ReadLine()??"";
    
    public static string ReadUserInput(string message)
        => Console.ReadLine()??"";
    
}

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

// Big boss these views to handle menus 
public abstract class Menu
{
    public string Title { get; set; }
    protected Dictionary<int, Delegate> _options;

    public virtual void Display()
    {
        while (true)
        {
            DisplayMenu();
            if (int.TryParse(Console.ReadLine(), out int input) && input == 0) break;
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



public class MainMenu : Menu
{
    public MainMenu()
    {
        Title = "Main Menu";
        _options = new Dictionary<int,Delegate>
        {
            { 1, () => { Console.WriteLine("Student Management");} },
            { 2, () => { Console.WriteLine("Trainer Management");} },
            { 3, ()=> { Console.WriteLine("Course Management");} },
            { 5, () => { Console.WriteLine("Scheduling Management");} }
        };
    }
    

    protected override void DisplayMenu()
    {
       
    }
}

public class PaginationView<T>  : Menu where T : class
{

    PaginationView(string title = "Pagination View")
    {
        Title = title;
        _options = new Dictionary<int, Delegate>
        {
            {1,ListItems}, {1,SearchItems}
        };
        
    }

    public void ListItems(Func<QueryParams,Paging<T>> getItemsAction,Action<T> printItemsAction)
    {
        var isExit= false;
        while (!isExit)
        {
            var isValidPageSize=InputParser.TryParseInt(UserInputReader.ReadUserInput("Enter how many items you wish to have: "), out int itemsPerPage);
            if (!isValidPageSize)
            {
                Console.WriteLine("Invalid Number"); 
                Console.WriteLine("Enter a valid Number");
            }
            
            if(!TryParseUserPageSizeInput(itemsPerPage,out var queryParams))
                continue;
           
            isExit= ListItemsOperation(getItemsAction,printItemsAction,queryParams,false);;
        }
    }

    public void SearchItems(Func<QueryParams,Paging<T>> getItemsAction,Action<T> printItemsAction,string searchIndecatorMessage)
    {
        var isExit= false;
        while (!isExit)
        {
            var isValidPageSize=InputParser.TryParseInt(UserInputReader.ReadUserInput("Enter how many items you wish to have: "), out int itemsPerPage);
            if (!isValidPageSize)
            {
                Console.WriteLine("Invalid Number"); 
                Console.WriteLine("Enter a valid Number");
            }
            
            if(!TryParseUserPageSizeInput(itemsPerPage,out var queryParams))
                continue;
            var whatToBeSearchedBy= UserInputReader.ReadUserInput(searchIndecatorMessage);
            if (string.IsNullOrWhiteSpace(whatToBeSearchedBy))
            {
                Console.WriteLine("Invalid Input");
                Console.WriteLine("Enter a valid Input");
                continue;
            }
            queryParams.Search= whatToBeSearchedBy;
            isExit= ListItemsOperation(getItemsAction,printItemsAction,queryParams,true);;
        }
    }

    private bool TryParseUserPageSizeInput(int pageSize,out QueryParams validatedQueryParams)
    {
        
        var queryParams= new QueryParams();
        queryParams.PageNumber = 1;
        queryParams.PageSize = pageSize;
        if (queryParams.PageSize <= 0)
        {
            validatedQueryParams = new QueryParams();
            return false;
        }
        validatedQueryParams= queryParams;
        return true;
    }

    private bool ListItemsOperation(Func<QueryParams,Paging<T>> getItemsAction,Action<T> printItemsAction, QueryParams queryParams ,bool isSearch=false)
    {
        var newQueryParam = new QueryParams { PageNumber = queryParams.PageNumber, PageSize = queryParams.PageSize, Search = isSearch?queryParams.Search:"" }; 
        
        var isExit= false;
        while (!isExit)
        {
            
            var result = getItemsAction(newQueryParam);
            
            Console.WriteLine(result.TotalItems); 
            
            Console.WriteLine(result.PageNumber); 
            
            Console.WriteLine(result.PageSize);

            foreach (var item in result.Result)
            {
                printItemsAction(item);
            } 
            
            Console.WriteLine("Press 0 to exit or press any other number to continue");
            
            var userInput= UserInputReader.ReadUserInput();

            if (!InputParser.TryParseInt(userInput, out int input))
            {
                Console.WriteLine("Invalid input"); Console.WriteLine("Enter a valid input"); continue;
            }

            switch (input)
            {
                case 0: 
                    isExit = true; 
                    break; 
                default:
                    newQueryParam.PageNumber++;
                    break;
            }
        }

        return false;
    } 
    
    
    
    protected override void DisplayMenu() { throw new NotImplementedException(); }
}

public class StudentManagementMenu : Menu
{
    private readonly StudentEnrollmentService _studentEnrollmentService;
    public StudentManagementMenu()
    {
        Title = "Student Management";
        _options = new Dictionary<int, Delegate>
        {
            { 1,  CreateStudent },
            { 2, ListStudents },
            { 3, SearchStudentByEmail },
            { 4, EnrollStudentInCourse },
            { 5, ViewStudentProgressInCode },
        };
        _studentEnrollmentService = new StudentEnrollmentService(new LmsContext(),new EventBuss(),new SystemClock(),new Validator<Student>());
    }
    
    protected override void DisplayMenu()
    {
        throw new NotImplementedException();
    }


    public void CreateStudent(Func<StudentDto,string> createStudent)
    {
        Console.WriteLine("Create Student");
        var studentEmail= UserInputReader.ReadUserInput("Enter Student Email");
        var studentFirstName= UserInputReader.ReadUserInput("Enter Student First Name");
        var studentLastName= UserInputReader.ReadUserInput("Enter Student Last Name");
        var studentDto= new StudentDto
        {
            Email = new Email(studentEmail),
            FullName = new FullName(studentFirstName,studentLastName),
            Status = StudentStatus.Active
        };
        
        createStudent(studentDto);
    }

    public void ListStudents()
    {
        Console.WriteLine("List Students");
        var queryParam = new QueryParams
        {
            PageNumber = 1,
            PageSize = 5,
        };
        var isExit= false;
        while (!isExit)
        {
            var isValidPageSize=InputParser.TryParseInt(UserInputReader.ReadUserInput("Enter how many items you wish to have: "), out int itemsPerPage);
            if (!isValidPageSize && itemsPerPage <= 0)
            {
                Console.WriteLine("Invalid Page Size");
                Console.WriteLine("Enter a valid Page Size");
                continue;
            }
            queryParam.PageSize= itemsPerPage;
            var result = _studentEnrollmentService.ListStudents(queryParam);
            Console.WriteLine(result.TotalItems);
            Console.WriteLine(result.PageNumber);
            Console.WriteLine(result.PageSize);
            foreach (var studentDto in result.Result)
            {
                Console.WriteLine(studentDto.FullName);
            }
            Console.WriteLine("Press 0 to exit or press any other number to continue");

            var userInput= UserInputReader.ReadUserInput();
            
            if (!InputParser.TryParseInt(userInput, out int input))
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine("Enter a valid input");
                continue;
            }
            switch (input)
            {
                case 0: 
                    isExit = true;
                    break;
                default:
                    queryParam.PageNumber++;
                    break;
            }
        }
    }
    
    public void SearchStudentByEmail()
    {
        
        
        var queryParam = new QueryParams
        {
            PageNumber = 1,
            PageSize = 5,
        };
        
        var isExit= false;
        while (!isExit)
        {
            var emailToBeSearched= UserInputReader.ReadUserInput("Enter Student Email To search It: ");
            queryParam.Search= emailToBeSearched;
            var isValidPageSize=InputParser.TryParseInt(UserInputReader.ReadUserInput("Enter how many items you wish to have: "), out int itemsPerPage);
            queryParam.PageSize= itemsPerPage;
            if (!isValidPageSize && itemsPerPage <= 0)
            {
                Console.WriteLine("Invalid Page Size");
                Console.WriteLine("Enter a valid Page Size");
                continue;
            }
            var result = _studentEnrollmentService.ListStudents(queryParam);
            Console.WriteLine(result.TotalItems);
            Console.WriteLine(result.PageNumber);
            Console.WriteLine(result.PageSize);
            foreach (var studentDto in result.Result)
            {
                Console.WriteLine(studentDto.FullName);
            }
            Console.WriteLine("Press 0 to exit or press any other number to continue");

            var userInput= UserInputReader.ReadUserInput();
            
            if (!InputParser.TryParseInt(userInput, out int input))
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine("Enter a valid input");
                continue;
            }
            switch (input)
            {
                case 0: 
                    isExit = true;
                    break;
                default:
                    queryParam.PageNumber++;
                    break;
            }
        }
    }

    public void EnrollStudentInCourse()
    {
        Console.WriteLine("Enroll Student In Course");
        Console.WriteLine("Search or choose the student you wish to enroll by copying and pasting the student id:");
        SearchStudentByEmail();
        var studentId= UserInputReader.ReadUserInput("Enter Student Id: ");
        
        _studentEnrollmentService.EnrollStudentInCourse();
    }
    
    
    public void ViewStudentProgressInCode()
    {
        Console.WriteLine("View Student Progress In Code");
    }
}





