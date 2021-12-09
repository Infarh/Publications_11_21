using Microsoft.EntityFrameworkCore;
using Publications.DAL.Context;
using Publications.Domain.Entities.Base;
using Publications.Interfaces.Repositories;

namespace Publications.DAL.Repositories;

public class DbRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly PublicationsDB _db;

    protected DbSet<T> Set { get; }

    public DbRepository(PublicationsDB db)
    {
        _db = db;
        Set = _db.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken Cancel = default)
    {
        return await Set.ToArrayAsync(Cancel).ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(int id, CancellationToken Cancel = default)
    {
        return await Set.FirstOrDefaultAsync(item => item.Id == id, Cancel).ConfigureAwait(false);
    }

    public async Task<int> AddAsync(T item, CancellationToken Cancel = default)
    {
        await Set.AddAsync(item, Cancel).ConfigureAwait(false);
        await _db.SaveChangesAsync(Cancel);
        return item.Id;
    }

    public async Task<bool> UpdateAsync(T item, CancellationToken Cancel = default)
    {
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
        return true;
    }

    public async Task<bool> DeleteAsync(T item, CancellationToken Cancel = default)
    {
        var entity = await GetAsync(item.Id, Cancel).ConfigureAwait(false);
        if (entity is null) return false;
        _db.Entry(entity).State = EntityState.Deleted;
        await _db.SaveChangesAsync(Cancel);

        return true;
    }
}