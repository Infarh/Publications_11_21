using Publications.DAL.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PublicationsDB>(opt => opt
    .UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

using(var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PublicationsDB>();
    //var publications = await db.Publications.ToArrayAsync();
    var id = 5;
    var some_publication = await db.Publications.FirstOrDefaultAsync(p => p.Id == id);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
