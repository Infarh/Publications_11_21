using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Publications.DAL;
using Publications.DAL.Context;
using Publications.Domain.Entities;
using Publications.MVC.Models;

namespace Publications.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _Logger;

    public HomeController(ILogger<HomeController> Logger) => _Logger = Logger;

    public async Task<IActionResult> Index(/*[FromServices] IUnitOfWork Work, [FromServices] PublicationsDB db*/)
    {
        //using (var transaction = await Work.BeginTransaction())
        //{
        //    db.Persons.Add(new Person { });

        //    await Work.SaveChanges();

        //    await Work.CommitTransaction();
        //}

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
