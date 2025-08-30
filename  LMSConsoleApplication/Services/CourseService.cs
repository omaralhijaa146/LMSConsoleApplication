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
    public CourseService(LmsContext lmsContext, IEventBuss eventBuss,IClock clock)
    {
        _lmsContext = lmsContext;
        _eventBuss = eventBuss;
        _clock = clock;
    }

    private bool CourseExists(Func<Course, bool> condition)
    {
        return _lmsContext.Courses.Any(condition);
    }
    
    public string CreateCourse(CourseDto course)
    {
        if(CourseExists(x=>x.Name==course.Name))
            throw new ArgumentException("Course already exists");
        
        var courseToAdd= new Course(course.Name, course.Description);;
        _lmsContext.Courses.Add(courseToAdd);
        return courseToAdd.Id.ToString();
    }
    
    public string EditCourse(string courseId,CourseDto course)
    {
        
        // TODO Refactor Course update
        if(course.Id!= courseId) throw new ArgumentException("Course id does not match");
        
        if(!CourseExists(x=>x.Id==Guid.Parse(courseId))) throw new ArgumentException("Course does not exists");
        
        var courseToUpdate = _lmsContext.Courses.FirstOrDefault(c=>c.Id==Guid.Parse(courseId));
        
        var courseTeachers= _lmsContext.Trainers.Where(x=>x.Courses.Any(c=>c.Id==Guid.Parse(courseId))).Select(x=>new Trainer(
            x.FullName.FirstName,x.FullName.LastName,x.Email.Value)).ToList();
        
        var courseStudents = _lmsContext.Students.Where(s => s.Enrollments.Any(x => x.CourseId == Guid.Parse(courseId))).Select(x=>x.Id).ToList();
        
        var courseEnrollments = _lmsContext.Enrollments.Where(x=>x.CourseId==Guid.Parse(courseId)&& courseStudents.Contains(x.StudentId)).Select(x=>new Enrollment(x.CourseId,x.StudentId,x.EnrolledAt)).ToList();
        var newCourseModules = courseToUpdate.Modules.Select(x => new Module(x.Id,x.Title,x.DurationInMinutes,x.Order,x.Completed,x.Optional,x.Session)).ToList();
        var courseSessions= _lmsContext.Sessions.Where(x=>x.CourseId==Guid.Parse(courseId)).ToList();
        var courseAnnouncements= _lmsContext.Announcements.Where(x=>x.CourseId==Guid.Parse(courseId)).ToList();
        
       var newCourse = new Course(course.Name, course.Description);
       
       /*var newCourse = new Course(course.Name, course.Description,  courseTeachers,newCourseModules, courseEnrollments,courseAnnouncements,courseSessions);*/
       
       _lmsContext.Courses.Add(newCourse);

       var courseTrainersIds=courseTeachers.Select(x=>x.Id).ToList();
       
       _lmsContext.Trainers.Where(x=>courseTrainersIds.Contains(x.Id)).ToList().ForEach(x=>x.Courses.Add(newCourse));
       
       _lmsContext.Trainers.Where(x=>courseTrainersIds.Contains(x.Id)).ToList().ForEach(x=>x.Courses.Remove(courseToUpdate));
       
       _lmsContext.Enrollments.Where(x=>courseStudents.Contains(x.StudentId)).ToList().ForEach(x=>x.CourseId=newCourse.Id);

       _lmsContext.Sessions.Where(x => x.CourseId == Guid.Parse(courseId)).ToList().ForEach(x => x = new Session(newCourse.Id,newCourse.Modules.FirstOrDefault(m=>m.Session.Id==x.Id).Id,courseTeachers.FirstOrDefault(t=>t.Id==x.TrainerId).Id,x.Location,x.TimeRange));
       //TODO : update announcements 
       // TODO 
       _lmsContext.Courses.Remove(courseToUpdate);
       
       return newCourse.Id.ToString();
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
    
    public void AddModule(string courseId,Module module)
    {
        //TODO create Module DTO
        //TODO validate module
        var listOfErrors=_moduleValidator.Validate(module).ToList();
        if (listOfErrors.Any())
        {
            string error="";
            throw new ArgumentException(listOfErrors.Aggregate(error, (s, s1) => error = s + s1 + "\n"));
        }
        
        if (string.IsNullOrWhiteSpace(module.Title))
            throw new ArgumentException("Invalid title");
        if(module.DurationInMinutes<=0)
            throw new InvalidOperationException("Module duration must be greater than 0 minutes");
        var originalModule = _lmsContext.Courses.FirstOrDefault(x => x.Id == Guid.Parse(courseId))?.Modules?.FirstOrDefault(x=>x.Title==module.Title);
        
        if(originalModule!=null) throw new InvalidOperationException("Module already exists");
        
        var course = _lmsContext.Courses.FirstOrDefault(x=>x.Id==Guid.Parse(courseId));
        var moduleInTheSameOrder = course?.Modules?.FirstOrDefault(x=>x.Order==module.Order);
        
        if(moduleInTheSameOrder!=null)
            throw new InvalidOperationException("Module with the same order already exists");
        
        course?.Modules?.Add(module);
    }
    public (string moduleId, int order) OrderModules(string courseId,string moduleId,int order)
    {
        void ShiftModules(List<Module> modules, Predicate<Module> condition)
        {
            foreach (var m in modules)
            {
                if (condition(m))
                    m.Order--;
            }
        }
        
        var course = RecordsFinder.FindEntity(_lmsContext.Courses, Guid.Parse(courseId));
        if(course is null) throw new ArgumentException("Course does not exists");
        var orderedModules=course.Modules.OrderBy(x => x.Order).ToList();
        var module = orderedModules.FirstOrDefault(x => x.Id == Guid.Parse(moduleId));
        if(module is null) throw new ArgumentException("Module does not exists");
        var currentOrder = module.Order;
        
        var newOrder = Math.Max(1,Math.Min(order,course.Modules.Count));
        if(currentOrder==newOrder) 
            throw new ArgumentException("Module order is already set");
        
        if (newOrder <currentOrder)
        {
            ShiftModules(orderedModules,m=>m.Order >= newOrder && m.Order < currentOrder);
            /*foreach (var m in orderedModules)
            {
                if(m.Order >= newOrder && m.Order < currentOrder)
                    m.Order++;
            }*/
        }else if (newOrder > currentOrder)
        {
            ShiftModules(orderedModules,m=>m.Order > currentOrder && m.Order <= newOrder);
            /*foreach (var m in orderedModules)
            {
                if (m.Order > currentOrder && m.Order <= newOrder)
                    m.Order--;
            }*/
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

        /*var schedulingPolicy = new SchedulingPolicy(_lmsContext);
        var isValid= schedulingPolicy.ValidateTrainerSessionSchedule(course.Sessions.FirstOrDefault(x=>x.TrainerId==Guid.Parse(trainerId)&&x.CourseId==Guid.Parse(courseId)));
        if(!isValid)
            throw new ArgumentException("cannot schedule trainer");*/
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