using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Publications.Domain.Entities;
using Publications.Interfaces;
using Publications.Interfaces.Repositories;
using Publications.MVC.ViewModels;

namespace Publications.MVC.Controllers;

public class PublicationsController : Controller
{
    private readonly IPublicationManager _PublicationManager;
    private readonly IRepository<Publication> _Publications;
    private readonly IRepository<Place> _Places;
    private readonly IRepository<Person> _Authors;
    private readonly ILogger<PublicationsController> _Logger;

    public PublicationsController(
        IPublicationManager PublicationManager,
        IRepository<Publication> Publications, 
        IRepository<Place> Places, 
        IRepository<Person> Authors, 
        ILogger<PublicationsController> Logger)
    {
        _PublicationManager = PublicationManager;
        _Publications = Publications;
        _Places = Places;
        _Authors = Authors;
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
            var publications = await _Publications.GetAllAsync();
            return View(publications);
        }
        catch (Exception e)
        {
            LogError(e);
            throw;
        }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Places = (await _Places.GetAllAsync()).Select(p => p.Name);
        ViewBag.Authors = (await _Authors.GetAllAsync()).Select(a => $"{a.LastName} {a.Name[0]}. {a.Patronymic}");

        return View(
            "Edit", new PublicationEditViewModel
            {
                Date = DateTime.Now
            });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var publication = await _Publications.GetAsync(id);
        if (publication is null)
            return NotFound();

        var model = new PublicationEditViewModel
        {
            Id = publication.Id,
            Name = publication.Name,
            Date = publication.Date,
            Annotation = publication.Annotation,
            Authors = string.Join(", ", publication.Authors.Select(a => $"{a.LastName} {a.Name[0]}. {a.Patronymic}")),
            Place = publication.Place.Name,
        };

        ViewBag.Places = (await _Places.GetAllAsync()).Select(p => p.Name);
        ViewBag.Authors = (await _Authors.GetAllAsync()).Select(a => $"{a.LastName} {a.Name[0]}. {a.Patronymic}");

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PublicationEditViewModel Model)
    {
        if (!ModelState.IsValid)
            return View(Model);

        if (Model.Id == 0)
        {
            var authors = Model.Authors.Split(",");
            var place_name = Model.Place;

            var new_publication = new Publication
            {
                Name = Model.Name,
                Date = Model.Date,
                Annotation = Model.Annotation,
                Place = null,
                //Authors = 
            };


            // создать новую публикацию
        }
        else
        {
            // отредактировать
        }


        return RedirectToAction(nameof(Index));
    }
}