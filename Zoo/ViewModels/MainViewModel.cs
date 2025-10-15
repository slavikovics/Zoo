using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Zoo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;

    [ObservableProperty] private bool _isBackEnabled;

    [ObservableProperty] private bool _isAddEnabled = true;
    
    [ObservableProperty] private bool _areAnimalsEnabled;
    
    [ObservableProperty] private bool _areAnimalTypesEnabled = true;
    
    [ObservableProperty] private bool _areWinterPlacesEnabled = true;
    
    [ObservableProperty] private bool _areDietsEnabled = true;
    
    [ObservableProperty] private bool _areDietTypesEnabled = true;
    
    [ObservableProperty] private bool _areEmployeesEnabled = true;
    
    [ObservableProperty] private bool _areHabitatsEnabled = true;
    
    [ObservableProperty] private bool _areReptileInfosEnabled = true;

    private readonly List<bool> _navItems;
    
    private readonly INavigationService? _navigationService;

    private void EnableAllNavItems()
    {
        for (int i = 0; i < _navItems.Count; i++)
        {
            _navItems[i] = true;
        }
    }

    public MainViewModel()
    {
        _navigationService = App.ServiceProvider?.GetService<INavigationService>();
        CurrentViewModel = _navigationService?.NavigateTo<PetsViewModel>();
        
        _navItems = [IsBackEnabled, IsAddEnabled, AreAnimalsEnabled, AreAnimalTypesEnabled, AreWinterPlacesEnabled, 
            AreDietsEnabled, AreDietTypesEnabled, AreEmployeesEnabled, AreHabitatsEnabled, AreReptileInfosEnabled];
    }

    [RelayCommand]
    private void NavigateToDiets()
    {
        CurrentViewModel = _navigationService?.NavigateTo<DietsViewModel>();
    }
    
    [RelayCommand]
    private void NavigateToEmployees()
    {
        CurrentViewModel = _navigationService?.NavigateTo<EmployeesViewModel>();
    }
}