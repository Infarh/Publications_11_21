using Publications.Domain.Entities;

namespace Publications.Interfaces.Repositories;

public interface IPersonsRepository : INamedRepository<Person>
{
    Task<Person?> GetByFIOAsync(string LastName, string Name, string Patronymic, CancellationToken Cancel = default);

    Task<Person?> GetByFIOAsync(string LastName, char Name, char Patronymic, CancellationToken Cancel = default);
}