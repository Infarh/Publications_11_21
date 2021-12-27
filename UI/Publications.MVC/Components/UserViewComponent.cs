using Microsoft.AspNetCore.Mvc;

namespace Publications.MVC.Components
{
    //[ViewComponent]
    public class UserViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => User.Identity!.IsAuthenticated
            ? View("Registred")
            : View();

        //public async Task<IViewComponentResult> InvokeAsync() => View();
    }
}
