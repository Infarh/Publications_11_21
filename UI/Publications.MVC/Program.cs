using Microsoft.AspNetCore.Identity;
using Publications.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Publications.DAL;
using Publications.Domain.Entities.Identity;
using Publications.MVC.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllersWithViews().AddRazorRuntimeCompilation();

services.AddDbContext<PublicationsDB>(opt => opt
    .UseSqlServer(builder.Configuration.GetConnectionString("Default")));
services.AddTransient<IUnitOfWork, EFUnitOfWork<PublicationsDB>>();

services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<PublicationsDB>()
   .AddDefaultTokenProviders();

services.Configure<IdentityOptions>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;
#endif

    opt.User.RequireUniqueEmail = false;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";

    opt.Lockout.AllowedForNewUsers = false;
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});

services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "Publications";

    opt.Cookie.HttpOnly = true;

    opt.ExpireTimeSpan = TimeSpan.FromDays(10);

    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";

    opt.SlidingExpiration = true;
});

var app = builder.Build();

using(var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PublicationsDB>();

    await db.Database.MigrateAsync(); // Создание БД в случае её отсутствия и приведение её к последнему состоянию в плане миграций

    //var publications = await db.Publications.ToArrayAsync();
    var id = 5;
    var some_publication = await db.Publications.FirstOrDefaultAsync(p => p.Id == id);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseBrowserLink();
}

app.UseMiddleware<TestMiddleware>();

//app.UseHttpsRedirection();
app.UseStaticFiles(/*new StaticFileOptions{ ServeUnknownFileTypes = true }*/);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

app.Run();
