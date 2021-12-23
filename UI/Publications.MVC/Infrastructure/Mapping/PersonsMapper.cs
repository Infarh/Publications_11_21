using Publications.Domain.Entities;
using Publications.MVC.ViewModels;

namespace Publications.MVC.Infrastructure.Mapping;

public static class PersonsMapper
{
    public static AuthorViewModel? ToView(this Person? person) => person is null
        ? null
        : new AuthorViewModel
        {
            Id = person.Id,
            LastName = person.LastName,
            Name = person.Name,
            Patronymic = person.Patronymic,
        };

    public static Person? FromView(this AuthorViewModel? person) => person is null
        ? null
        : new Person
        {
            Id = person.Id,
            LastName = person.LastName,
            Name = person.Name,
            Patronymic = person.Patronymic,
        };

    public static IEnumerable<AuthorViewModel?> ToView(this IEnumerable<Person?> persons) => persons.Select(ToView);

    public static IEnumerable<Person?> FromView(this IEnumerable<AuthorViewModel?> persons) => persons.Select(FromView);
}