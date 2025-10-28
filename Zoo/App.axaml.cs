using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DatabaseUtils;
using DatabaseUtils.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zoo.ViewModels;
using Zoo.Views;

namespace Zoo;

public partial class App : Application
{
    public static ServiceProvider? ServiceProvider { get; private set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private string GetConnectionString()
    {
        var settings = File.ReadAllText("database.json");
        return JsonSerializer.Deserialize<Dictionary<string, string>>(settings)!["ConnectionString"];
    }

    private void RegisterUserServices()
    {
        var connectionString = Task.Run(GetConnectionString).Result;
        ServiceCollection serviceCollection = new ();
        
        serviceCollection.AddSingleton<IDatabaseConnectionFactory>(_ => 
            new NpgsqlConnectionFactory(connectionString));
        serviceCollection.AddSingleton<INavigationService, NavigationService>();
        serviceCollection.AddSingleton<MainViewModel>();
        
        serviceCollection.AddTransient<PetsViewModel>();
        serviceCollection.AddTransient<DietsViewModel>();
        serviceCollection.AddTransient<EmployeesViewModel>();
        serviceCollection.AddTransient<AddAnimalViewModel>();
        serviceCollection.AddTransient<AddDietViewModel>();
        serviceCollection.AddTransient<AddEmployeeViewModel>();
        
        serviceCollection.AddSingleton<ISelectService, SelectService>();
        
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        RegisterUserServices();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            var mainWindow = new MainWindow();
                
            var navService = ServiceProvider?.GetRequiredService<INavigationService>();
            mainWindow.DataContext = navService?.NavigateTo<MainViewModel>();
                
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}