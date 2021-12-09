using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publications.Domain.Entities.Base;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
}

public abstract class NamedEntity : Entity
{
    [Required]
    public string Name { get; set; }
}