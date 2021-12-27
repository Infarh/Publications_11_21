using Microsoft.Extensions.Logging;

using Publications.Domain.Entities;
using Publications.Interfaces;
using Publications.Interfaces.Repositories;

namespace Publications.Services;

public class DatabasePublicationManager : IPublicationManager
{
    private readonly IRepository<Publication> _Publications;
    private readonly IPersonsRepository _Persons;
    private readonly INamedRepository<Place> _Places;
    private readonly ILogger<DatabasePublicationManager> _Logger;

    public IRepository<Publication> Publications => _Publications;
    public IRepository<Person> Authors => _Persons;
    public IRepository<Place> Places => _Places;

    public DatabasePublicationManager(
        IRepository<Publication> Publications,
        IPersonsRepository Persons,
        INamedRepository<Place> Places,
        ILogger<DatabasePublicationManager> Logger)
    {
        _Publications = Publications;
        _Persons = Persons;
        _Places = Places;
        _Logger = Logger;
    }

    private static readonly char[] __AuthorSeparators = ",. ".ToCharArray();

    public async Task<Publication> CreateAsync(string Name, string Annotation, DateTime Date, string PlaceName, params string[] AuthorNames)
    {
        var authors = new List<Person>();

        foreach (var author in AuthorNames)
        {
            // Иванов И.И., Петров П. П., Сидоров С С

            var fio = author.Split(__AuthorSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (fio.Length != 3)
                throw new FormatException("Строка автора имела неверный формат - не содержит указания фамилии, имени и отчества через пробел, либо запятую")
                {
                    Data =
                    {
                        { "Author Name", author }
                    }
                };

            var author_last_name = fio[0];
            var author_name = fio[1];
            var author_patronymic = fio[2];
            var person = await _Persons.GetByFIOAsync(author_last_name, author_name, author_patronymic);

            if (person is null)
            {
                person = new Person
                {
                    LastName = author_last_name,
                    Name = author_name,
                    Patronymic = author_patronymic,
                };

                await _Persons.AddAsync(person);

            }

            authors.Add(person);
        }

        if (await _Places.GetByNameAsync(PlaceName) is not { } place)
        {
            place = new Place
            {
                Name = PlaceName
            };

            await _Places.AddAsync(place);
        }

        var publication = new Publication
        {
            Name = Name,
            Date = Date,
            Annotation = Annotation,
            Place = place,
            Authors = authors,
        };

        await _Publications.AddAsync(publication);

        return publication;
    }
}