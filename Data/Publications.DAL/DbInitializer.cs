using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Publications.DAL.Context;
using Publications.Domain.Entities;
using Publications.Interfaces;

namespace Publications.DAL;

public class DbInitializer : IDbInitializer
{
    private readonly PublicationsDB _db;
    private readonly ILogger<DbInitializer> _Logger;

    public DbInitializer(PublicationsDB db, ILogger<DbInitializer> Logger)
    {
        _db = db;
        _Logger = Logger;
    }

    public async Task<bool> DeleteAsync(CancellationToken Cancel = default)
    {
        var timer = Stopwatch.StartNew();
        Cancel.ThrowIfCancellationRequested();

        try
        {
            _Logger.LogInformation("Удаление БД...");

            var result = await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);

            _Logger.LogInformation("БД удалена успешно за {0}c", timer.Elapsed.TotalSeconds);

            return result;
        }
        catch (OperationCanceledException e)
        {
            _Logger.LogInformation("Операция удаления БД была прервана");
            throw;
        }
        catch (Exception e)
        {
            _Logger.LogError(e, "Ошибка при удалении БД: {0}", e.Message);
            throw;
        }
    }

    public async Task Initialize(bool RemoveAtStart = false, CancellationToken Cancel = default)
    {
        var timer = Stopwatch.StartNew();
        _Logger.LogInformation("Инициализация БД...");

        try
        {
            if (RemoveAtStart)
                await DeleteAsync(Cancel).ConfigureAwait(false);

            var pending_migration = await _db.Database.GetPendingMigrationsAsync(Cancel).ConfigureAwait(false);
            if (pending_migration.Any())
            {
                _Logger.LogInformation("Выполняется миграция БД...");
                await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);
                _Logger.LogInformation("Миграция БД успешно завершена");
            }

            await InitializePublications(Cancel).ConfigureAwait(false);

            _Logger.LogInformation("Инициализация БД выполнена успешно за {0}c", timer.Elapsed.TotalSeconds);
        }

        catch (OperationCanceledException e)
        {
            _Logger.LogInformation("Операция инициализации БД была прервана");
            throw;
        }
        catch (Exception e)
        {
            _Logger.LogError(e, "Ошибка при инициализации БД: {0}", e.Message);
            throw;
        }
    }

    private async Task InitializePublications(CancellationToken Cancel)
    {
        if (await _db.Persons.AnyAsync(Cancel).ConfigureAwait(false))
        {
            _Logger.LogInformation("БД не требует инициализации тестовыми данными");
            return;
        }

        var authors = Enumerable.Range(1, 5).Select(
            i => new Person
            {
                LastName = $"LastName-{i}",
                Name = $"Name-{i}",
                Patronymic = $"Patronymic-{i}",
            }).ToArray();

        var places = Enumerable.Range(1, 10).Select(
                i => new Place
                {
                    Name = $"Place-{i}",
                })
           .ToArray();

        var rnd = new Random();

        var publications = Enumerable.Range(1, 50).Select(i => new Publication
        {
            Name = $"Publication-{i}",
            Date = DateTime.Now.AddYears(-rnd.Next(5, 21)),
            Authors = Enumerable.Range(1, rnd.Next(1, 4))
               .Select(_ => authors[rnd.Next(authors.Length)])
               .Distinct()
               .ToList(),
            Place = places[rnd.Next(places.Length)],
        });

        await _db.Persons.AddRangeAsync(authors, Cancel).ConfigureAwait(false);
        await _db.Places.AddRangeAsync(places, Cancel).ConfigureAwait(false);
        await _db.Publications.AddRangeAsync(publications, Cancel).ConfigureAwait(false);

        await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
    }
}