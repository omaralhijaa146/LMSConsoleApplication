namespace LMSConsoleApplication.Domain.Entities;

public abstract class Entity:IEntity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public abstract bool IsValid();
    public bool IsInvalid()=>!IsValid();
}