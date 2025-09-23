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
using Avalonia.Markup.Xaml;
using DatabaseUtils;
using Microsoft.Extensions.DependencyInjection;
using Zoo.ViewModels;
using Zoo.Views;

namespace Zoo;

public partial class App : Application
{
    public static ServiceProvider? ServiceProvider { get; set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async Task<string> GetConnectionString()
    {
        var settings = await File.ReadAllTextAsync("database.json");
        return JsonSerializer.Deserialize<Dictionary<string, string>>(settings)!["ConnectionString"];
    }

    private async Task RegisterUserServices()
    {
        var connectionString = await GetConnectionString();
        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IDatabaseConnectionFactory>(_ => 
            new NpgsqlConnectionFactory(connectionString));
        serviceCollection.AddTransient<PetsViewModel>();
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        RegisterUserServices();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceProvider?.GetService<PetsViewModel>()
            };
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