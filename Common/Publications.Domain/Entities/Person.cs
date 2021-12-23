using Publications.Domain.Entities.Base;

namespace Publications.Domain.Entities;

public class Person : NamedEntity
{
    /// <summary>Фамилия</summary>
    public string LastName { get; set; } = null!;

    /// <summary>Отчество</summary>
    public string Patronymic { get; set; } = null!;

    public override string ToString() => $"{Id} {LastName} {Name} {Patronymic}";
}
