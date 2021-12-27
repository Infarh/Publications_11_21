using Publications.Interfaces.Base.Entities;

namespace Publications.Interfaces.Repositories;

public interface INamedRepository<T> : IRepository<T> where T : class, INamedEntity
{
    Task<T?> GetByNameAsync(string Name, CancellationToken Cancel = default);
}