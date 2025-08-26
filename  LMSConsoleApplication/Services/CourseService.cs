using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Events;
using LMSConsoleApplication.Domain.Specifications;
using LMSConsoleApplication.DTO;
using LMSConsoleApplication.Helpers;

namespace LMSConsoleApplication.Services;

public class CourseService
{
    private readonly LmsContext _lmsContext;
    private readonly IEventBuss _eventBuss;
    private readonly IClock _clock;
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
        
        var courseToAdd= new Course{Description = course.Description, Name = course.Name,Id = Guid.NewGuid()};
        _lmsContext.Courses.Add(courseToAdd);
        return courseToAdd.Id.ToString();
    }

    public string EditCourse(string courseId,CourseDto course)
    {
        if(course.Id!= courseId) throw new ArgumentException("Course id does not match");
        
        if(!CourseExists(x=>x.Id==Guid.Parse(courseId))) throw new ArgumentException("Course does not exists");
        
        var courseToUpdate = _lmsContext.Courses.FirstOrDefault(c=>c.Id==Guid.Parse(courseId));
        
        var courseTeachers= _lmsContext.Trainers.Where(x=>x.Courses.Any(c=>c.Id==Guid.Parse(courseId))).Select(x=>new Trainer(
            x.FullName.FirstName,x.FullName.LastName,x.Email.Value)).ToList();
        var courseStudents = _lmsContext.Students.Where(s => s.Enrollments.Any(x => x.CourseId == Guid.Parse(courseId))).Select(x=>x.Id).ToList();
        
        var courseEnrollments = _lmsContext.Enrollments.Where(x=>x.CourseId==Guid.Parse(courseId)&& courseStudents.Contains(x.StudentId)).Select(x=>new Enrollment
        {
            Id = x.Id,
            CourseId = x.CourseId,
            StudentId = x.StudentId,
            EnrolledAt = x.EnrolledAt
        }).ToList();
        
       var newCourse = new Course{Description = course.Description, Name = course.Name,Modules = courseToUpdate.Modules.Select(x=>new Module
       {
           Id = x.Id,
           DurationInMinutes = x.DurationInMinutes,
           Order=x.Order,
           Title = x.Title,
           Optional = x.Optional,
           Session = x.Session,
       }).ToList(),Trainers = courseTeachers,Enrollments = courseEnrollments};
       _lmsContext.Courses.Add(newCourse);
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