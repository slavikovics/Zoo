using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Text.Json;
using Avalonia.Markup.Xaml;
using DatabaseUtils;
using DatabaseUtils.Queries;
using DatabaseUtils.Repositories;
using DatabaseUtils.TableNameResolver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zoo.ViewModels;
using Zoo.Views;

namespace Zoo;

public partial class App : Application
{
    public static ServiceProvider? ServiceProvider { get; private set; }

    private DatabaseConfigDto _databaseConfig;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private string GetConnectionString()
    {
        return _databaseConfig.ConnectionString;
    }

    private Dictionary<string, string> GetTableConfig()
    {
        return _databaseConfig.TableNames;
    }

    private void LoadDatabaseConfig()
    {
        var settings = File.ReadAllText("database.json");
        _databaseConfig = JsonSerializer.Deserialize<DatabaseConfigDto>(settings)!;
    }

    private void RegisterUserServices()
    {
        LoadDatabaseConfig();
        var connectionString = GetConnectionString();
        ServiceCollection serviceCollection = new();
        serviceCollection.AddSingleton<ITableNameResolver>(_ =>
            new TableNameResolver(GetTableConfig(), GetConnectionString()));

        serviceCollection.AddSingleton<IDatabaseConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(connectionString));

        serviceCollection.AddSingleton<IAnimalsRepository, AnimalsRepository>();
        serviceCollection.AddSingleton<IEmployeesRepository, EmployeesRepository>();
        serviceCollection.AddSingleton<IDietsRepository, DietsRepository>();
        serviceCollection.AddSingleton<IDietTypesRepository, DietTypesRepository>();

        serviceCollection.AddSingleton<INavigationService, NavigationService>();
        serviceCollection.AddSingleton<MainViewModel>();

        serviceCollection.AddTransient<PetsViewModel>();
        serviceCollection.AddTransient<DietsViewModel>();
        serviceCollection.AddTransient<DietTypesViewModel>();
        serviceCollection.AddTransient<EmployeesViewModel>();
        
        serviceCollection.AddTransient<AddAnimalViewModel>();
        serviceCollection.AddTransient<AddDietViewModel>();
        serviceCollection.AddTransient<AddEmployeeViewModel>();
        serviceCollection.AddTransient<AddDietTypeViewModel>();

        serviceCollection.AddTransient<UpdateDietTypeViewModel>();

        serviceCollection.AddSingleton<ISelectService, SelectService>();
        serviceCollection.AddSingleton<IDeleteService, DeleteService>();

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