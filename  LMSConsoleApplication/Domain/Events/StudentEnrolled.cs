namespace LMSConsoleApplication.Domain.Events;

public record StudentEnrolled(string StudentId, string CourseId, DateTime EnrolledAt);