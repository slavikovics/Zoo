using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Zoo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;

    [ObservableProperty] private bool _isBackEnabled;

    [ObservableProperty] private bool _isAddEnabled;
    
    private readonly INavigationService? _navigationService;

    public MainViewModel()
    {
        _navigationService = App.ServiceProvider?.GetService<INavigationService>();
        NavigateToPets();
    }

    [RelayCommand]
    private void NavigateToPets()
    {
        CurrentViewModel = _navigationService?.NavigateTo<PetsViewModel>();
        IsAddEnabled = true;
        IsBackEnabled = false;
    }

    [RelayCommand]
    private void NavigateToAddPet()
    {
        CurrentViewModel = _navigationService?.NavigateTo<AddAnimalViewModel>();
        IsAddEnabled = false;
        IsBackEnabled = true;
    }

    [RelayCommand]
    private void NavigateToDiets()
    {
        CurrentViewModel = _navigationService?.NavigateTo<DietsViewModel>();
        IsAddEnabled = true;
        IsBackEnabled = false;
    }
    
    [RelayCommand]
    private void NavigateToAddDiet()
    {
        CurrentViewModel = _navigationService?.NavigateTo<DietEditViewModel>();
        IsAddEnabled = false;
        IsBackEnabled = true;
    }
    
    [RelayCommand]
    private void NavigateToEmployees()
    {
        CurrentViewModel = _navigationService?.NavigateTo<EmployeesViewModel>();
        IsAddEnabled = true;
        IsBackEnabled = false;
    }
    
    [RelayCommand]
    private void NavigateToAddEmployee()
    {
        CurrentViewModel = _navigationService?.NavigateTo<EmployeeEditViewModel>();
        IsAddEnabled = false;
        IsBackEnabled = true;
    }

    [RelayCommand]
    private void NavigateToAnimalSearch()
    {
        
    }

    [RelayCommand]
    private void NavigateToFamilyPairs()
    {
        
    }

    [RelayCommand]
    private void Add()
    {
        switch (CurrentViewModel)
        {
            case PetsViewModel: NavigateToAddPet(); break;
            case DietsViewModel: NavigateToAddDiet(); break;
            case EmployeesViewModel: NavigateToAddEmployee(); break;
        }
    }

    [RelayCommand]
    private void Back()
    {
        switch (CurrentViewModel)
        {
            case AddAnimalViewModel: NavigateToPets(); break;
            case DietEditViewModel: NavigateToDiets(); break;
            case EmployeeEditViewModel: NavigateToEmployees(); break;
        }
    }
}