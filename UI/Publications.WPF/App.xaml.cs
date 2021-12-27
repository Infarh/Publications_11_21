﻿using System;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;

using Publications.Interfaces;
using Publications.WPF.Commands;
using Publications.WPF.Services;
using Publications.WPF.Services.Interfaces;
using Publications.WPF.ViewModels;
using Publications.WPF.Views.Windows;

namespace Publications.WPF;

public partial class App
{
    private static IHost? __Host;

    public static IHost Hosting => __Host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static IServiceProvider Services => Hosting.Services;

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
       .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        //services.AddDbContext<PublicationsDB>(opt => opt.UseSqlServer(host.Configuration.GetConnectionString("default")));

        //services.AddScoped<IPublicationManager, DatabasePublicationManager>();

        services.AddScoped<MainWindowViewModel>();
        services.AddScoped<SettingsWindowViewModel>();
        services.AddScoped<SettingsWindow>(s => new SettingsWindow { DataContext = s.GetRequiredService<SettingsWindowViewModel>() });
        services.AddTransient<IUserDialog, UserDialog>();
        services.AddTransient<SettingsCommand>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        var host = Hosting;

        await host.StartAsync().ConfigureAwait(true); // !!! ConfigureAwait(true) !!!

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using var host = Hosting;

        base.OnExit(e);

        await host.StopAsync();
    }
}