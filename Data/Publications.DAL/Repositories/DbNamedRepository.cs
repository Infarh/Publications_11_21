using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Publications.DAL.Context;
using Publications.Interfaces.Base.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.DAL.Repositories;

public class DbNamedRepository<T> : DbRepository<T>, INamedRepository<T>
    where T : class, INamedEntity
{
    public DbNamedRepository(PublicationsDB db, ILogger<DbNamedRepository<T>> Logger) : base(db, Logger)
    {
    }

    public async Task<T?> GetByNameAsync(string Name, CancellationToken Cancel = default)
    {
        return await Items.FirstOrDefaultAsync(item => item.Name == Name, Cancel).ConfigureAwait(false);
    }
}
