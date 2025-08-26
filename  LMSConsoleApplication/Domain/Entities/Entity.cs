namespace LMSConsoleApplication.Domain.Entities;

public class Entity:IEntity
{
    public Guid Id { get; set; }= Guid.NewGuid();
}