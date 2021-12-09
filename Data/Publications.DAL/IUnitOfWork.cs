using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Publications.DAL;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken Cancel = default);

    Task<IDbContextTransaction> BeginTransaction(CancellationToken Cancel = default);

    Task CommitTransaction(CancellationToken Cancell = default);
}

public class EFUnitOfWork<T> : IUnitOfWork
    where T : DbContext
{
    private readonly T _db;

    public EFUnitOfWork(T db) { _db = db; }

    public async Task SaveChanges(CancellationToken Cancel = default)
    {
        await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
    }

    public async Task<IDbContextTransaction> BeginTransaction(CancellationToken Cancel = default)
    {
        return await _db.Database.BeginTransactionAsync(Cancel).ConfigureAwait(false);
    }

    public async Task CommitTransaction(CancellationToken Cancell = default)
    {
        await _db.Database.CommitTransactionAsync(Cancell).ConfigureAwait(false);
    }
}