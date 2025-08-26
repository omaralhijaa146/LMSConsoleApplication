using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Domain.Specifications;
using LMSConsoleApplication.DTO;

namespace LMSConsoleApplication.Services;

public class TrainerService
{
    private readonly LmsContext _lmsContext;
    private readonly IEventBuss _eventBuss;
    private readonly IClock _clock;
    
    public TrainerService(LmsContext context, IEventBuss eventBuss,IClock clock)
    {
        _lmsContext = context;
        _eventBuss = eventBuss;
        _clock = clock;
    }


    public Paging<TrainerDto> ListTrainers(QueryParams queryParam)
    {
        var trainersSpecs = new TrainerSpecificationByNameOrderByName(queryParam);
        var evaluatedQuery= SpecificationEvaluator<Trainer>.GetQuery(_lmsContext.Trainers,trainersSpecs);
        var trainers = evaluatedQuery.Select(x=>new TrainerDto
        {
          FullName = x.FullName,
          Id = x.Id.ToString(),
          Email = x.Email
        }).ToList();
        var pagedResult = new Paging<TrainerDto>(queryParam.PageNumber,queryParam.PageSize,trainers.Count,trainers);
        return pagedResult;
    }
    
    public Paging<TrainerDto> SearchTrainers(QueryParams queryParam)
    {
        return ListTrainers(queryParam);
    }

    public void CreateTrainer(TrainerDto trainerDto)
    {
        var trainerAlreadyExists= _lmsContext.Trainers.Any(x=>x.Email.Value==trainerDto.Email.Value);
        if (trainerAlreadyExists)
            throw new ArgumentException("Trainer already exists");
        
        var trainerToAdd= new Trainer(trainerDto.FullName.FirstName,trainerDto.FullName.LastName,trainerDto.Email.Value);
        _lmsContext.Trainers.Add(trainerToAdd);
    }


    public void CreateTrainerAvailabilityWindow(string trainerId,AvailabiltyWindowDto windowDto)
    {
        var availabilityAlreadyExists= _lmsContext.Trainers.FirstOrDefault(x=>x.Id==Guid.Parse(trainerId))?.AvailabilityWindows.Any(x=>x.Start==windowDto.Start&&x.End==windowDto.End);
        if (availabilityAlreadyExists.HasValue && availabilityAlreadyExists.Value)
            throw new ArgumentException("Trainer availability window already exists");
        
        var overlaps= _lmsContext.Trainers.FirstOrDefault(x=>x.Id==Guid.Parse(trainerId))?.AvailabilityWindows.Any(x=>x.Start < windowDto.End && x.End > windowDto.Start);
        if (overlaps.HasValue && overlaps.Value)
            throw new InvalidOperationException("Trainer availability window overlaps with another");
        var availabilityToBeAdded = new AvailabilityWindow
        {
            End = windowDto.End,
            Start = windowDto.Start
        };
        _lmsContext.Trainers.FirstOrDefault(x=>x.Id==Guid.Parse(trainerId))?.AvailabilityWindows.Add(availabilityToBeAdded);
    }
    
    public void DeleteTrainerAvailabilityWindow(string trainerId,AvailabiltyWindowDto windowDto)
    {
        var availabilityToBeDeleted = _lmsContext.Trainers.FirstOrDefault(x=>x.Id==Guid.Parse(trainerId))?.AvailabilityWindows.FirstOrDefault(x=>x.Start==windowDto.Start&&x.End==windowDto.End);
        if (availabilityToBeDeleted is null)
            throw new ArgumentException("Trainer availability window does not exists");
        _lmsContext.Trainers.FirstOrDefault(x=>x.Id==Guid.Parse(trainerId))?.AvailabilityWindows.Remove(availabilityToBeDeleted);
    }
    
    public List<SessionDto> ListTrainerSessions(QueryParams queryParam,string trainerId)
    {
        var trainer = _lmsContext.Trainers.FirstOrDefault(x=>x.Id==Guid.Parse(trainerId));
        if (trainer is null)
            throw new ArgumentException("Trainer does not exists");
        var sessions = trainer.Sessions.Select(x=>new SessionDto
        {
            Id = x.Id.ToString(),
            CourseId = x.CourseId.ToString(),
            ModuleId = x.ModuleId.ToString(),
            TrainerId = x.TrainerId.ToString(),
            Location = x.Location,
            TimeRange = x.TimeRange
        }).ToList();

        var result = sessions;
        return result;
    }
}



/*
 * Trainers

Create/List/Search trainers

Maintain trainer availability (simple daily time ranges)

View trainer schedule (their sessions)
 */