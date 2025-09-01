using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Enums;
using LMSConsoleApplication.Domain.Events;
using LMSConsoleApplication.Domain.Requirements;
using LMSConsoleApplication.Domain.Specifications;
using LMSConsoleApplication.DTO;
using LMSConsoleApplication.Utilties;

namespace LMSConsoleApplication.Services;


public class StudentEnrollmentService
    {
        private readonly LmsContext _lmsContext;
        private readonly IEventBuss _eventBus;
        private readonly IValidator<Student> _studentValidator;
        private readonly IClock _clock;

        public StudentEnrollmentService(LmsContext lmsContext, IEventBuss eventBus,IClock clock)
        {
            _lmsContext = lmsContext;
            _eventBus = eventBus;
            _clock = clock;
        }

        public string CreateStudent(StudentDto studentDto)
        {
            
            if (_lmsContext.Students.Any(s => s.Email.Value == studentDto.Email.Value))
                throw new ArgumentException("A student with the provided email already exists.");
            var studentToAdd = new Student(studentDto.FullName.FirstName, studentDto.FullName.LastName,
                studentDto.Email.Value, studentDto.Status);
            var isValidStudent= _studentValidator.Validate(studentToAdd).ToList();
            if(isValidStudent.Count>0)
                throw new ArgumentException(isValidStudent.Aggregate("",(s,s1)=>s+s1+"\n"));
            
            _lmsContext.Students.Add(studentToAdd);
            return studentToAdd.Id.ToString();
        }

        public Paging<StudentDto> ListStudents(QueryParams queryParam)
        {
            var studentSpecs = new StudentSpecificationByNameOrderByByName(queryParam);
            var evaluatedQuery = SpecificationEvaluator<Student>.GetQuery(_lmsContext.Students, studentSpecs);
            var students = evaluatedQuery.Select(x=>new StudentDto
            {
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status
            }).ToList();
            
            var pagedResult= new Paging<StudentDto>(queryParam.PageNumber,queryParam.PageSize,students.Count,students);
            return pagedResult;
        }

        public Paging<StudentDto> SearchStudentByEmail(QueryParams queryParams)
        {
            return ListStudents(queryParams);
        }

        public (string studentId,string courseId) EnrollStudentInCourse(string studentId, string courseId)
        {
            var student = _lmsContext.Students.FirstOrDefault(s => s.Id == Guid.Parse(studentId));
            var course = _lmsContext.Courses.FirstOrDefault(c => c.Id == Guid.Parse(courseId));

            if (student == null)
                throw new ArgumentException("Student not found.");

            if (course == null)
                throw new ArgumentException("Course not found.");

            if (_lmsContext.Enrollments.Any(e => e.StudentId == Guid.Parse(studentId) && e.CourseId == Guid.Parse(courseId)))
                throw new InvalidOperationException("The student is already enrolled in this course.");
            
            
            var enrollment = new Enrollment(Guid.Parse(studentId), Guid.Parse(courseId), _clock.UtcNow);
            
            _lmsContext.Enrollments.Add(enrollment);
            _lmsContext.Courses.FirstOrDefault(c => c.Id == Guid.Parse(courseId))?.Enrollments?.Add(enrollment);
            _lmsContext.Students.FirstOrDefault(s => s.Id == Guid.Parse(studentId))?.Enrollments.Add(enrollment);
            _eventBus.Publish(new StudentEnrolled(studentId, courseId, DateTime.UtcNow));
            return (studentId,courseId);
        }

        public double ViewStudentProgressInCode(string studentId,string courseId)
        {
            var student = _lmsContext.Students.FirstOrDefault(s => s.Id == Guid.Parse(studentId));

            if (student == null)
                throw new ArgumentException("Student not found.");
            
            var courseAndEnrollment = _lmsContext.Courses.Join(_lmsContext.Enrollments,x=>x.Id,x=>x.CourseId,(c,e)=>new {Course= c, Enrollment=e,Modules=c.Modules}).FirstOrDefault(c => c.Course.Id == Guid.Parse(courseId) && c.Enrollment.StudentId== Guid.Parse(studentId));
            
            if (courseAndEnrollment == null)
                throw new ArgumentException("Course not found.");
            if(courseAndEnrollment.Modules is null || !courseAndEnrollment.Modules.Any())
                throw new ArgumentException("Course has no modules");
            
            var totalModules = courseAndEnrollment.Modules.Count;
            var completedModules = courseAndEnrollment.Modules.Count(m => m.Completed==ModuleCompleteStatus.Completed);
            var progress = totalModules > 0 ? (completedModules * 100.0 / totalModules) : 0;
            
            return progress;
        }
    }



