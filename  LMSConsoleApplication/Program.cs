using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Enums;
using LMSConsoleApplication.Domain.Events;
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
        eventBus.Subscribe<TrainerAssignedToCourse>(x=>notifier.InfoNotify($"Trainer {x.TrainerId} assigned to course {x.CourseId}"));
        eventBus.Subscribe<StudentEnrolled>(x=>notifier.InfoNotify($"Trainer {x.CourseId} assigned to course {x.CourseId}"));
        var context = new LmsContext();
        var courseService = new CourseService(context,eventBus,clock);
        var trainerService = new TrainerService(context,eventBus,clock);
        trainerService.CreateTrainer(new TrainerDto
        {
            FullName = new FullName("omar","kamal"),
            Email = new Email("omar@omar.com"),
        });
       
        var courseId= courseService.CreateCourse(new CourseDto{Name = "C#",Description = "C# course"});
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

        var moduleId = Guid.NewGuid();
        courseService.AddModule(courseId,new Module
        {
            Id = moduleId,
            Optional = true,
            Order = 1,
            DurationInMinutes = 20,
            Title = "Module 1",
        });

        var studentEnrollmentService = new StudentEnrollmentService(context,eventBus,clock);
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
       var sessionService = new SchedulingService(context,eventBus,clock);;
       var sessionId= sessionService.CreateSession(new SessionDto
        {
            CourseId = courseId,
            ModuleId = moduleId.ToString(),
            TrainerId = trainer.Id,
            Location = "1",
            TimeRange = new TimeRange(DateTime.Now,DateTime.Now.AddHours(1))
        });
       
        var sessionId2= sessionService.CreateSession(new SessionDto
        {
            CourseId = courseId,
            ModuleId = moduleId.ToString(),
            TrainerId = trainer.Id,
            Location = "1",
            TimeRange = new TimeRange(DateTime.Now.AddMinutes(60),DateTime.Now.AddMinutes(-20))
        });
       context.Courses.FirstOrDefault(x=>x.Id==Guid.Parse(courseId)).Sessions.ForEach(x=>Console.WriteLine(x.TimeRange.Start));;
       
    }
}

// Big boss these views to handle menus 
public abstract class Menu
{
    public string Title { get; set; }
    protected Dictionary<int, Delegate> _options;
    public abstract void Display();
    protected abstract void HandleInput(int input);

    protected virtual void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine($"=== {Title} ===");
        foreach (var option in _options)
        {
            Console.WriteLine($"{option.Key}. {option.Value}");
        }
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");
    }
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

    public override void Display()
    {
        while (true)
        {
            DisplayMenu();
            if (int.TryParse(Console.ReadLine(), out int input) && input == 0) break;
            HandleInput(input);
        }
    }

    protected override void HandleInput(int input)
    {
        if (_options.TryGetValue(input, out var action))
        {
            var convertedAction =(Action)action;
            convertedAction();
        }
    }
}


