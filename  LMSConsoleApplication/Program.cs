using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Enums;
using LMSConsoleApplication.Domain.Events;
using LMSConsoleApplication.Domain.Requirements;
using LMSConsoleApplication.DTO;
using LMSConsoleApplication.Helpers;
using LMSConsoleApplication.Services;
using LMSConsoleApplication.Views;

namespace LMSConsoleApplication;

class Program
{
    static void Main(string[] args)
    {
        /*var eventBus = new EventBuss();
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
       context.Courses.FirstOrDefault(x=>x.Id==Guid.Parse(courseId)).Sessions.ForEach(x=>Console.WriteLine(x.TimeRange.Start));;*/

        var appManager = new AppManager();
        appManager.Run();
    }
}

public class MainMenuController
{
    private readonly MainMenu _mainMenu;

    public MainMenuController(Action studentManagementController)
    {
        _mainMenu = new MainMenu(studentManagementController, () => { }, () => { }, () => { });
    }

    
    public void Display()
    {
        _mainMenu.Display();
    }
    
}

public class StudentManagementMenuController
{
    private readonly StudentManagementMenu _studentManagementMenu;
    private readonly StudentEnrollmentService _studentEnrollmentService;
    private readonly CourseService _courseService;

    public StudentManagementMenuController(StudentEnrollmentService studentEnrollmentService,CourseService courseService)
    {
        _studentEnrollmentService = studentEnrollmentService;
        _courseService = courseService;
        var studentManagementHandlers = new StudentManagementHandlers
        {
            PaginationHandlers = new PaginationViewHandlers<StudentDto>
            {
                GetItemsAction = _studentEnrollmentService.ListStudents,
                PrintItemsAction = (std) => {Console.WriteLine($"Student {std.FullName.FirstName} {std.FullName.LastName} {std.Email.Value} with Id {std.Id}"); },
                SearchIndecatorMessage = "Search by student email"
            },
            CoursePaginationHandlers = new PaginationViewHandlers<CourseDto>
            {
                GetItemsAction = _courseService.ListCourses,
                PrintItemsAction = (course) => {Console.WriteLine($"Course {course.Name} with Id {course.Id}"); },
                SearchIndecatorMessage = "Search by course name"
            },
            CreateStudent = _studentEnrollmentService.CreateStudent,
            EnrollmentAction = _studentEnrollmentService.EnrollStudentInCourse,
            ViewStudentProgressInCode = _studentEnrollmentService.ViewStudentProgressInCode,
        };
        _studentManagementMenu = new StudentManagementMenu(studentManagementHandlers);
    }
    
    
    public void Display()
    {
        _studentManagementMenu.Display();
    }
}

public class AppManager
{
    private  LmsContext _lmsContext;
    private  StudentEnrollmentService _studentEnrollmentService;
    private  SchedulingService _schedulingService;
    private  CourseService _courseService;
    private  TrainerService _trainerService;
    private  IEventBuss _eventBuss;
    private  IClock _clock;
    private  INotifier _notifier;
    private  IValidator<Course> _courseValidator;
    private  IValidator<Module> _moduleValidator;
    private  IValidator<Trainer> _trainerValidator;
    private  IValidator<Student> _studentValidator;
    private  IValidator<Session> _sessionValidator;
    private  IValidator<AvailabilityWindow> _availabilityWindowValidator;
    private  IValidator<Enrollment> _enrollmentValidator;
    private MainMenuController _mainMenuController;
    private StudentManagementMenuController _studentManagementMenuController;
    
    public AppManager()
    {
        InitilizeEventingUtils();
        InitializeValidators();
        InitializeContextAndServices();
        AddSubscribes();
        InitializeControllers();
    }
    
    public void Run()
    {
        _mainMenuController.Display();
    }


    private void InitializeControllers()
    {
        _studentManagementMenuController = new StudentManagementMenuController(_studentEnrollmentService,_courseService);;
        _mainMenuController = new MainMenuController(_studentManagementMenuController.Display);
    }

    private void AddSubscribes()
    {
        _eventBuss.Subscribe<TrainerAssignedToCourse>(x=>_notifier.InfoNotify($"Trainer {x.TrainerId} assigned to course {x.CourseId}"));
        _eventBuss.Subscribe<StudentEnrolled>(x=>_notifier.InfoNotify($"Trainer {x.CourseId} assigned to course {x.CourseId}"));
        _eventBuss.Subscribe<SessionScheduled>(x=>_notifier.InfoNotify($"Session Scheduled {x.sessionDto.CourseId}  {x.sessionDto.Id} {x.sessionDto.ModuleId} {x.sessionDto.TrainerId}"));
    }

    private void InitilizeEventingUtils()
    {
        _eventBuss = new EventBuss();
        _clock = new SystemClock();
        _notifier = new Notifier();
    }
    private void InitializeValidators()
    {
        _courseValidator = new Validator<Course>().AddRule<CreateCourseRequirement>();
        _studentValidator = new Validator<Student>().AddRule<StudentRequirement>();
        _sessionValidator = new Validator<Session>().AddRule<SessionRequirement>();
        _trainerValidator = new Validator<Trainer>().AddRule<TrainerRequirement>();
        _moduleValidator= new Validator<Module>().AddRule<ModuleRequirement>(); ;
    }
    private void InitializeContextAndServices()
    {
        _lmsContext = new LmsContext();
        _courseService = new CourseService(_lmsContext,_eventBuss,_clock,_courseValidator,_moduleValidator);
        _trainerService = new TrainerService(_lmsContext,_eventBuss,_clock,_availabilityWindowValidator,_trainerValidator);
        _studentEnrollmentService = new StudentEnrollmentService(_lmsContext,_eventBuss,_clock,_studentValidator);
        _schedulingService = new SchedulingService(_lmsContext,_eventBuss,_clock,_sessionValidator);
        
    }
    
}






