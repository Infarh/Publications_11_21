using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Publications.DAL.Context;
using Publications.Interfaces.Base.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.DAL.Repositories;

public class DbRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly PublicationsDB _db;
    private readonly ILogger<DbRepository<T>> _Logger;

    protected DbSet<T> Set { get; }

    protected virtual IQueryable<T> Items => Set;

    public DbRepository(PublicationsDB db, ILogger<DbRepository<T>> Logger)
    {
        _db = db;
        _Logger = Logger;
        Set = _db.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken Cancel = default)
    {
        var result = await Items.ToArrayAsync(Cancel).ConfigureAwait(false);
        return result;
    }

    public async Task<T?> GetAsync(int id, CancellationToken Cancel = default)
    {
        return await Items.FirstOrDefaultAsync(item => item.Id == id, Cancel).ConfigureAwait(false);
    }

    public async Task<int> AddAsync(T item, CancellationToken Cancel = default)
    {
        await Set.AddAsync(item, Cancel).ConfigureAwait(false);
        await _db.SaveChangesAsync(Cancel);
        _Logger.LogInformation("Объект {0} добавлен в БД", item);
        return item.Id;
    }

    public async Task<bool> UpdateAsync(T item, CancellationToken Cancel = default)
    {
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);

        _Logger.LogInformation("Информация об объекте {0} обновлена в БД", item);

        return true;
    }

    public async Task<bool> DeleteAsync(T item, CancellationToken Cancel = default)
    {
        var entity = await GetAsync(item.Id, Cancel).ConfigureAwait(false);
        if (entity is null)
        {
            _Logger.LogInformation("Удаляемый объект {0} не найден в БД", item);
            return false;
        }
        _db.Entry(entity).State = EntityState.Deleted;
        await _db.SaveChangesAsync(Cancel);

        _Logger.LogInformation("Объект {0} удалён из БД", item);

        return true;
    }
}