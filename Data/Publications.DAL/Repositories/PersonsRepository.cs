using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Publications.DAL.Context;
using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.DAL.Repositories;

public class PersonsRepository : DbNamedRepository<Person>, IPersonsRepository
{
    public PersonsRepository(PublicationsDB db, ILogger<DbNamedRepository<Person>> Logger) : base(db, Logger)
    {
    }

    public async Task<Person?> GetByFIOAsync(string LastName, string Name, string Patronymic, CancellationToken Cancel = default)
    {
        return await Items.FirstOrDefaultAsync(person => 
                    person.LastName == LastName && 
                    person.Name == Name && 
                    person.Patronymic == Patronymic, 
                Cancel)
           .ConfigureAwait(false);
    }

    public async Task<Person?> GetByFIOAsync(string LastName, char Name, char Patronymic, CancellationToken Cancel = default)
    {
        return await Items.FirstOrDefaultAsync(person => 
                    person.LastName == LastName && 
                    person.Name.First() == Name && 
                    person.Patronymic.First() == Patronymic, 
                Cancel)
           .ConfigureAwait(false);
    }
}
