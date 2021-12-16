using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Publications.DAL.Context;
using Publications.Interfaces;
using Publications.Services;

namespace Publications.ConsoleTests;

class Program
{
    private static IHost? __Host;

    public static IHost Hosting => __Host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
       .ConfigureHostConfiguration(opt => opt.AddJsonFile("otherconfigs.json", true, false))
       .ConfigureAppConfiguration(opt => opt.AddJsonFile("otherconfigs.json", false, true))
       .ConfigureAppConfiguration(opt => opt.AddXmlFile("appsettings.xml", true, true))
       .ConfigureAppConfiguration(opt => opt.AddIniFile("appsettings.ini", true, true))
       .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services.AddDbContext<PublicationsDB>(opt => opt.UseSqlServer(host.Configuration.GetConnectionString("default")));

        services.AddScoped<IPublicationManager, DatabasePublicationManager>();
    }

    public static async Task Main(string[] args)
    {
        using var host = Hosting;

        await host.StartAsync();

        var products = Enumerable.Range(1, 10)
           .Select(i => new Product(i, $"Product-{i}", $"Description-{i}", i * 1000))
           .ToArray();

        const string report_template = "CatalogTemplate.docx";
        var report = new ProductsWordReport(new(report_template))
        {
            CatalogName = "Компьютеры",
            ProductsCount = 120,
            CreationDate = DateTime.Now
        };
        var file = report.Create("computers.docx");

        Process.Start(new ProcessStartInfo(file.FullName) { UseShellExecute = true });

        // получаем главный сервис приложения и запускаем работу в нём (асинхронно).

        await host.StopAsync();

        //var service_collection = new ServiceCollection();
        //service_collection.AddSingleton("Hello world!");
        //service_collection.AddScoped<>()

        //var services = service_collection.BuildServiceProvider();


        //var ui = (IUserDialog)services.GetService(typeof(IUserDialog));
        //var ui = services.GetService<IUserDalog>()
        //var ui = services.GetRequiredService<IUserDialog>();
        //var sp = services.GetRequiredService<IServiceProvider>();

        //using (var scope = services.CreateScope())
        //{
        //    var scope_services = scope.ServiceProvider;
        //}


    }
}