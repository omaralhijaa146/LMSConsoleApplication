using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Events;
using LMSConsoleApplication.Domain.Requirements;
using LMSConsoleApplication.Domain.Specifications;
using LMSConsoleApplication.DTO;
using LMSConsoleApplication.Helpers;

namespace LMSConsoleApplication.Services;

public class CourseService
{
    private readonly LmsContext _lmsContext;
    private readonly IEventBuss _eventBuss;
    private readonly IClock _clock;
    private readonly IValidator<Course> _courseValidator;
    private readonly IValidator<Module> _moduleValidator;
    public CourseService(LmsContext lmsContext, IEventBuss eventBuss,IClock clock,IValidator<Course> courseValidator,IValidator<Module> moduleValidator)
    {
        _lmsContext = lmsContext;
        _eventBuss = eventBuss;
        _clock = clock;
        _courseValidator = courseValidator;
        _moduleValidator = moduleValidator;
    }

    private bool CourseExists(Func<Course, bool> condition)
    {
        return _lmsContext.Courses.Any(condition);
    }
    
    public string CreateCourse(CreateCourseDto course)
    {
        if(CourseExists(x=>x.Name==course.Name))
            throw new ArgumentException("Course already exists");
        var courseIsValid = _courseValidator.Validate(new Course(course.Name, course.Description)).ToList();
        if(courseIsValid.Count > 0)
            throw new ArgumentException(courseIsValid.Aggregate("",(s,s1)=>s+s1+"\n"));
        
        var courseToAdd= new Course(course.Name, course.Description);
        _lmsContext.Courses.Add(courseToAdd);
        return courseToAdd.Id.ToString();
    }

    public string EditCourse(string courseId, UpdateCourseDto courseUpdateDto)
    {

        if (courseUpdateDto.Id != courseId) throw new ArgumentException("Course id does not match");

        if (!CourseExists(x => x.Id == Guid.Parse(courseId))) throw new ArgumentException("Course does not exists");

        var courseToUpdate = _lmsContext.Courses.FirstOrDefault(c => c.Id == Guid.Parse(courseId));
       courseToUpdate.Name = courseUpdateDto.Name;
       courseToUpdate.Description = courseUpdateDto.Description;
       return courseToUpdate.Id.ToString();
    }

    public Paging<CourseDto> ListCourses(QueryParams queryParam)
    {
        var courseSpec = new CourseSpecificationByNameOrderByName(queryParam);
        var evaluatedQuery = SpecificationEvaluator<Course>.GetQuery( _lmsContext.Courses,courseSpec);
        var courses =evaluatedQuery.Select(x => new CourseDto
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            Description = x.Description,
            Trainers = x.Trainers.Select(x=>new TrainerDto
            {
                FullName = x.FullName,
                Id = x.Id.ToString(),
                Email = x.Email
            }).ToList()
        }).ToList();

        var pagedResult = new Paging<CourseDto>(queryParam.PageNumber,queryParam.PageSize,courses.Count,courses);
        return pagedResult;
    }
    
    public Paging<CourseDto> SearchCourses(QueryParams queryParam)
    {
        return ListCourses(queryParam);
    }
    
    public void AddModule(string courseId,CreateModuleDto moduleDto)
    {
        var course = _lmsContext.Courses.FirstOrDefault(x=>x.Id==Guid.Parse(courseId));
        if(course is null)
            throw new ArgumentException("Course does not exists");
        
        var moduleExists= course?.Modules?.FirstOrDefault(x=>x.Title==moduleDto.Title&&x.Order==moduleDto.Order);
        if(moduleExists is not null)
            throw new ArgumentException("Module already exists");

        var newModule = new Module(moduleDto.Title, moduleDto.DurationInMinutes, moduleDto.Order, moduleDto.Completed);
        var listOfErrors=_moduleValidator.Validate(newModule).ToList();
        if (listOfErrors.Any())
        {
            string error="";
            throw new ArgumentException(listOfErrors.Aggregate(error, (s, s1) => error = s + s1 + "\n"));
        }
        
        course?.Modules?.Add(newModule);
    }
    public (string moduleId, int order) OrderModules(string courseId,string moduleId,int order)
    {
        void ShiftModules(List<Module> modules, Predicate<Module> condition)
        {
            foreach (var m in modules)
            {
                if (condition(m))
                    m.Order--;
                else
                    m.Order++;
            }
        }
        
        var course = RecordsFinder.FindEntity(_lmsContext.Courses, Guid.Parse(courseId));
        
        if(course is null) throw new ArgumentException("Course does not exists");
        if(course.Modules is null) throw new ArgumentException("Course has no modules");
        
        var orderedModules=course.Modules.OrderBy(x => x.Order).ToList();
        var module = orderedModules?.FirstOrDefault(x => x.Id == Guid.Parse(moduleId));
        
        if(module is null) throw new ArgumentException("Module does not exists");
        
        var currentOrder = module.Order;
        
        var newOrder = Math.Max(1,Math.Min(order,course.Modules.Count));
        if(currentOrder==newOrder) 
            throw new ArgumentException("Module order is already set");
        
        if (newOrder <currentOrder)
        {
            ShiftModules(orderedModules,m=>m.Order >= newOrder && m.Order < currentOrder);
        }else if (newOrder > currentOrder)
        {
            ShiftModules(orderedModules,m=>m.Order > currentOrder && m.Order <= newOrder);
        }
        module.Order = newOrder;
        course.Modules = orderedModules.OrderBy(x=>x.Order).ToList();
        return (module.Id.ToString(),module.Order);
    }

    public (string courseId,string trainerId) AssignTrainer(string courseId,string trainerId)
    {
        var parsedCourseId = Guid.Parse(courseId);
        var course = RecordsFinder.FindEntity(_lmsContext.Courses, parsedCourseId);
        
        var parsedTrainerId = Guid.Parse(trainerId);
        var trainerRegistered = RecordsFinder.FindEntity(_lmsContext.Trainers,parsedTrainerId );
        
        if (course is null||trainerRegistered is null)
            throw new ArgumentException("Course or trainer does not exists");
        
        if((bool)course.Trainers?.Any(x=>x.Id==Guid.Parse(trainerId)))
            throw new InvalidOperationException("Trainer already assigned");
        
        course?.Trainers?.Add(trainerRegistered);
        trainerRegistered?.Courses?.Add(course);
        
        _eventBuss.Publish(new TrainerAssignedToCourse(trainerId,courseId,_clock.UtcNow));
        return (courseId,trainerId);
    }

    
}

/*
 * Create/Edit/List/Search courses

Add/Order modules under a course

Assign one or more trainers to a course
 */