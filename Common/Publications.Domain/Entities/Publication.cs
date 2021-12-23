using Publications.Domain.Entities.Base;

namespace Publications.Domain.Entities;

public class Publication : NamedEntity
{
    public string Annotation { get; set; }

    public DateTime Date { get; set; }

    public ICollection<Person> Authors { get; set; } = new HashSet<Person>();

    public Place Place { get; set; }
}