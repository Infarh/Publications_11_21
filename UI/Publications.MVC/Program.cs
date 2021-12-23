using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Publications.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Publications.DAL;
using Publications.DAL.Repositories;
using Publications.Domain.Entities;
using Publications.Domain.Entities.Identity;
using Publications.Interfaces;
using Publications.Interfaces.Repositories;
using Publications.MVC.Infrastructure.Autofac;
using Publications.MVC.Infrastructure.Middleware;
using Publications.Services;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<DatabasePublicationManager>()
       .As<IPublicationManager>()
       .InstancePerLifetimeScope();

    //container.RegisterAssemblyTypes(Assembly.Load("Publications.Services"))
    //   .Where(type => type.Namespace?.StartsWith("Publications.Services") == true)
    //   .As(type => type.GetInterfaces().FirstOrDefault(i => i.Name == "I" + type.Name)!);
    //.AsImplementedInterfaces();

    //container.RegisterModule<PublicationServicesModule>();
    //container.RegisterModule(new PublicationServicesModule());
    container.RegisterAssemblyModules(typeof(Program));

    //container.RegisterComposite<>()
});

var services = builder.Services;
services.AddControllersWithViews().AddRazorRuntimeCompilation();

services.AddDbContext<PublicationsDB>(opt => opt
    .UseSqlServer(builder.Configuration.GetConnectionString("Default")));
services.AddTransient<IUnitOfWork, EFUnitOfWork<PublicationsDB>>();
services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>)); // Регистрация обобщённого интерфейса. Контейнер сам подставит в <...> нужный тип!
services.AddScoped<IRepository<Publication>, PublicationsRepository>();
services.AddTransient<IDbInitializer, DbInitializer>();

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

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

//services.AddAutoMapper(typeof(Program));
services.AddAutoMapper(Assembly.GetEntryAssembly());

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db.Initialize(RemoveAtStart: false);
}

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

    app.UseSwagger();
    app.UseSwaggerUI();

    //app.UseBrowserLink();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

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
