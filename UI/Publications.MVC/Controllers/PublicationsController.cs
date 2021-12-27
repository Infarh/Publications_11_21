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
    private readonly ILogger<PublicationsController> _Logger;

    public PublicationsController(
        IPublicationManager PublicationManager,
        ILogger<PublicationsController> Logger)
    {
        _PublicationManager = PublicationManager;
        
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
            var publications = await _PublicationManager.Publications.GetAllAsync();
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
        ViewBag.Places = (await _PublicationManager.Places.GetAllAsync()).Select(p => p.Name);
        ViewBag.Authors = (await _PublicationManager.Authors.GetAllAsync()).Select(a => $"{a.LastName} {a.Name[0]}. {a.Patronymic}");

        return View(
            "Edit", new PublicationEditViewModel
            {
                Date = DateTime.Now
            });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var publication = await _PublicationManager.Publications.GetAsync(id);
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

        ViewBag.Places = (await _PublicationManager.Places.GetAllAsync()).Select(p => p.Name);
        ViewBag.Authors = (await _PublicationManager.Authors.GetAllAsync()).Select(a => $"{a.LastName} {a.Name[0]}. {a.Patronymic}");

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PublicationEditViewModel Model)
    {
        if (!ModelState.IsValid)
            return View(Model);

        if (Model.Id == 0)
        {
            var result = await _PublicationManager.CreateAsync(Model.Name, Model.Annotation, Model.Date, Model.Place, Model.Authors.Split(","));

        }
        else
        {
            // отредактировать
        }


        return RedirectToAction(nameof(Index));
    }
}