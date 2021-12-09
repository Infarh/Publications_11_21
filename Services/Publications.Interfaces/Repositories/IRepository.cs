using Publications.Domain.Entities.Base;

namespace Publications.Interfaces.Repositories;

public interface IRepository<T> where T : class, IEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken Cancel = default);

    async Task<T?> GetAsync(int id, CancellationToken Cancel = default)
    {
        var all = await GetAllAsync(Cancel).ConfigureAwait(false);
        return all.FirstOrDefault(item => item.Id == id);
    }

    Task<int> AddAsync(T item, CancellationToken Cancel = default);

    Task<bool> UpdateAsync(T item, CancellationToken Cancel = default);

    Task<bool> DeleteAsync(T item, CancellationToken Cancel = default);
}