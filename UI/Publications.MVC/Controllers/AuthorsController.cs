using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;
using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;
using Publications.MVC.ViewModels;

namespace Publications.MVC.Controllers;

public class AuthorsController : Controller
{
    private readonly IRepository<Person> _Persons;
    private readonly ILogger<AuthorsController> _Logger;

    public AuthorsController(IRepository<Person> Persons, ILogger<AuthorsController> Logger)
    {
        _Persons = Persons;
        _Logger = Logger;
    }

    private void LogError(Exception e, [CallerMemberName] string MethodName = null!)
    {
        _Logger.LogError(e, "Ошибка выполнении {0}", MethodName);
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var authors = await _Persons.GetAllAsync();
            var authors_view_models = authors.Select(
                a => new AuthorViewModel
                {
                    Id = a.Id,
                    LastName = a.LastName,
                    Name = a.Name,
                    Patronymic = a.Patronymic,
                });

            return View(authors_view_models);
        }
        catch (Exception e)
        {
            LogError(e);
            throw;
        }
    }
}