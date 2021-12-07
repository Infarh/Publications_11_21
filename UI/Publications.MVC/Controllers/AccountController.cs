using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Publications.Domain.Entities.Identity;
using Publications.MVC.ViewModels;

namespace Publications.MVC.Controllers
{
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
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout() => RedirectToAction("Index", "Home");
    }
}
