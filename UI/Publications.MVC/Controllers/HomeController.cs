using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Publications.DAL;
using Publications.DAL.Context;
using Publications.Domain.Entities;
using Publications.Interfaces;
using Publications.Interfaces.Repositories;
using Publications.MVC.Models;

namespace Publications.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _Logger;

    public HomeController(ILogger<HomeController> Logger) => _Logger = Logger;

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Publications([FromServices] IRepository<Publication> Publications)
    {
        var publications = await Publications.GetAllAsync();

        return View(publications);
    }

    public async Task<IActionResult> PublicationInfo(int id, [FromServices] IRepository<Publication> Publications)
    {
        var publication = await Publications.GetAsync(id);
        if (publication is null)
            return NotFound();
        return View(publication);
    }
}
