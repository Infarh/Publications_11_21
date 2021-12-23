using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.MVC.Controllers;

public class PublicationsController : Controller
{
    private readonly IRepository<Publication> _Publications;
    private readonly ILogger<PublicationsController> _Logger;

    public PublicationsController(IRepository<Publication> Publications, ILogger<PublicationsController> Logger)
    {
        _Publications = Publications;
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
}