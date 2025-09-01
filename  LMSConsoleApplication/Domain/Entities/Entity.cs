using LMSConsoleApplication.Domain.Requirements;

namespace LMSConsoleApplication.Domain.Entities;

public abstract class Entity:IEntity
{
    private Guid _id;
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get=>_id; init=>_id=ValidateId(value); }
    public abstract bool IsValid();
    public bool IsInvalid()=>!IsValid();

    protected Guid ValidateId(Guid id)
    {
        if(!new ValidGuidIdRequirement(id).IsMet())
            throw new ArgumentException("Invalid id");
        return id;
    }
}