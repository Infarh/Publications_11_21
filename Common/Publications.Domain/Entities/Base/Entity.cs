using Publications.Interfaces.Base.Entities;

namespace Publications.Domain.Entities.Base;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
}