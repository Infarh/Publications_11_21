using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.Interfaces
{
    public interface IPublicationManager
    {
        IRepository<Publication> Publications { get; }
        IRepository<Person> Authors { get; }
        IRepository<Place> Places { get; }

        Task<Publication> CreateAsync(string Name, string Annotation, DateTime Date, string Place, params string[] Authors);
    }
}
