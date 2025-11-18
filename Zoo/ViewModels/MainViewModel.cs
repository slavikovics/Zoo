using System.Threading.Tasks;
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
        CurrentViewModel = _navigationService?.NavigateTo<AddDietViewModel>();
        IsAddEnabled = false;
        IsBackEnabled = true;
    }

    [RelayCommand]
    private void NavigateToDietTypes()
    {
        CurrentViewModel = _navigationService?.NavigateTo<DietTypesViewModel>();
        IsAddEnabled = true;
        IsBackEnabled = false;
    }

    [RelayCommand]
    private void NavigateToAddDietType()
    {
        CurrentViewModel = _navigationService?.NavigateTo<AddDietTypeViewModel>();
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
        CurrentViewModel = _navigationService?.NavigateTo<AddEmployeeViewModel>();
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

    public async Task UpdateDietType(int id)
    {
        CurrentViewModel = _navigationService?.NavigateTo<UpdateDietTypeViewModel>();
        await (CurrentViewModel as UpdateDietTypeViewModel)!.InitializeAsync(id);
        IsAddEnabled = false;
        IsBackEnabled = true;
    }

    public async Task UpdateDiet(int id)
    {
        CurrentViewModel = _navigationService?.NavigateTo<UpdateDietViewModel>();
        await (CurrentViewModel as UpdateDietViewModel)!.InitializeAsync(id);
        IsAddEnabled = false;
        IsBackEnabled = true;
    }

    public async Task UpdateEmployee(int id)
    {
        CurrentViewModel = _navigationService?.NavigateTo<UpdateEmployeeViewModel>();
        await (CurrentViewModel as UpdateEmployeeViewModel)!.InitializeAsync(id);
        IsAddEnabled = false;
        IsBackEnabled = true;
    }

    public async Task UpdateAnimal(int id)
    {
        CurrentViewModel = _navigationService?.NavigateTo<UpdateAnimalViewModel>();
        await (CurrentViewModel as UpdateAnimalViewModel)!.InitializeAsync(id);
        IsAddEnabled = false;
        IsBackEnabled = true;
    }

    [RelayCommand]
    private void Add()
    {
        switch (CurrentViewModel)
        {
            case PetsViewModel: NavigateToAddPet(); break;
            case DietsViewModel: NavigateToAddDiet(); break;
            case EmployeesViewModel: NavigateToAddEmployee(); break;
            case DietTypesViewModel: NavigateToAddDietType(); break;
        }
    }

    [RelayCommand]
    private void Back()
    {
        switch (CurrentViewModel)
        {
            case AddAnimalViewModel: NavigateToPets(); break;
            case UpdateAnimalViewModel: NavigateToPets(); break;
            case AddDietViewModel: NavigateToDiets(); break;
            case UpdateDietViewModel: NavigateToDiets(); break;
            case AddEmployeeViewModel: NavigateToEmployees(); break;
            case UpdateEmployeeViewModel: NavigateToEmployees(); break;
            case AddDietTypeViewModel: NavigateToDietTypes(); break;
            case UpdateDietTypeViewModel: NavigateToDietTypes(); break;
        }
    }
}