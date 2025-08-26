namespace LMSConsoleApplication.Domain.Events;

public record TrainerAssignedToCourse(string TrainerId, string CourseId, DateTime DateAssigned);