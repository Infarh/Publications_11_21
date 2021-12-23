using System.ComponentModel.DataAnnotations;
using Publications.Interfaces.Base.Entities;

namespace Publications.Domain.Entities.Base;

public abstract class NamedEntity : Entity, INamedEntity
{
    [Required]
    public string Name { get; set; } = null!;
}
