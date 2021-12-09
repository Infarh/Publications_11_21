using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Publications.Domain.Entities.Base;

namespace Publications.Domain.Entities;

public class Publication : NamedEntity
{
    public DateTime Date { get; set; }

    public ICollection<Person> Authors { get; set; } = new HashSet<Person>();
}

public class Person : NamedEntity
{
    public string LastName { get; set; }

    public string Patronymic { get; set; }
}