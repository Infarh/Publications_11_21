using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Publications.Domain.Entities.Identity;
using Publications.MVC.ViewModels;

namespace Publications.MVC.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _UserManager;
    private readonly SignInManager<User> _SignInManager;
    private readonly ILogger<AccountController> _Logger;

    public AccountController(
        UserManager<User> UserManager, 
        SignInManager<User> SignInManager, 
        ILogger<AccountController> Logger)
    {
        _UserManager = UserManager;
        _SignInManager = SignInManager;
        _Logger = Logger;
    }

    public IActionResult Register() => View(new RegisterUserViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserViewModel Model)
    {
        if (!ModelState.IsValid) return View(Model);

        var user = new User
        {
            UserName = Model.UserName
        };

        var creation_result = await _UserManager.CreateAsync(user, Model.Password);

        if (creation_result.Succeeded)
        {
            //await _UserManager.AddToRoleAsync(user, "User");

            await _SignInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in creation_result.Errors)
            ModelState.AddModelError("", error.Description);

        _Logger.LogWarning("Ошибка регистрации нового пользователя {0}", 
            string.Join(",", creation_result.Errors.Select(e => e.Description)));

        return View(Model);
    }

    public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl });

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel Model)
    {
        if (!ModelState.IsValid) return View(Model);

        var login_result = await _SignInManager.PasswordSignInAsync(
            Model.UserName,
            Model.Password,
            Model.RememberMe,
            false);

        if (login_result.Succeeded)
        {
            return LocalRedirect(Model.ReturnUrl ?? "/");
        }

        ModelState.AddModelError("", "Ошибка ввода имени пользователя или пароля");

        return View(Model);
    }

    public async Task<IActionResult> Logout()
    {
        await _SignInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}