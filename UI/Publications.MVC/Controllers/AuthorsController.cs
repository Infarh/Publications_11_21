using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;
using Publications.MVC.Infrastructure.Mapping;
using Publications.MVC.ViewModels;

namespace Publications.MVC.Controllers;

public class AuthorsController : Controller
{
    private readonly IRepository<Person> _Persons;
    private readonly IMapper _Mapper;
    private readonly ILogger<AuthorsController> _Logger;

    public AuthorsController(IRepository<Person> Persons, IMapper Mapper, ILogger<AuthorsController> Logger)
    {
        _Persons = Persons;
        _Mapper = Mapper;
        _Logger = Logger;
    }

    private void LogError(Exception e, [CallerMemberName] string MethodName = null!) { _Logger.LogError(e, "Ошибка выполнении {0}", MethodName); }

    public async Task<IActionResult> Index()
    {
        try
        {
            var authors = await _Persons.GetAllAsync();
            //var authors_view_models = authors.Select(
            //    a => new AuthorViewModel
            //    {
            //        Id = a.Id,
            //        LastName = a.LastName,
            //        Name = a.Name,
            //        Patronymic = a.Patronymic,
            //    });

            var authors_view_models = authors.ToView();

            //var authors_view_models = _Mapper.Map<IEnumerable<AuthorViewModel>>(authors);

            return View(authors_view_models);
        }
        catch (Exception e)
        {
            LogError(e);
            throw;
        }
    }

    public async Task<IActionResult> Edit(int Id)
    {
        try
        {
            var person = await _Persons.GetAsync(Id);
            if (person is null)
                return NotFound();

            //return View(person.ToView());
            return View(_Mapper.Map<AuthorViewModel>(person));
        }
        catch (Exception e)
        {
            LogError(e);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AuthorViewModel Model)
    {
        if(Model.LastName == "Иванов")
            ModelState.AddModelError("LastName", "Не любим эту фамилию");

        if(Model.LastName == "Петров" && Model.Name == "Пётр" && Model.Patronymic == "Петрович")
            ModelState.AddModelError("", "Не любим этого человека");

        if (!ModelState.IsValid)
            return View(Model);

        try
        {
            var person = Model.FromView()!;

            await _Persons.UpdateAsync(person);

            return RedirectToAction(nameof(Index));

        }
        catch (Exception e)
        {
            LogError(e);
            throw;
        }
    }
}