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
    [ObservableProperty] private bool _areAnimalsEnabled;
    [ObservableProperty] private bool _areDietTypesEnabled;
    [ObservableProperty] private bool _areDietsEnabled;
    [ObservableProperty] private bool _areEmployeesEnabled;
    [ObservableProperty] private bool _isSearchEnabled;
    [ObservableProperty] private bool _areFamilyPairsEnabled;
    
    private readonly INavigationService? _navigationService;

    public MainViewModel()
    {
        _navigationService = App.ServiceProvider?.GetService<INavigationService>();
        _navigationService?.SetMainViewModel(this);
    }

    [RelayCommand]
    private void NavigateToPets()
    {
        _navigationService?.NavigateToPets();
    }

    [RelayCommand]
    private void NavigateToAddPet()
    {
        _navigationService?.NavigateToAddPet();
    }

    [RelayCommand]
    private void NavigateToDiets()
    {
        _navigationService?.NavigateToDiets();
    }

    [RelayCommand]
    private void NavigateToAddDiet()
    {
        _navigationService?.NavigateToAddDiet();
    }

    [RelayCommand]
    private void NavigateToDietTypes()
    {
        _navigationService?.NavigateToDietTypes();
    }

    [RelayCommand]
    private void NavigateToAddDietType()
    {
        _navigationService?.NavigateToAddDietType();
    }

    [RelayCommand]
    private void NavigateToEmployees()
    {
        _navigationService?.NavigateToEmployees();
    }

    [RelayCommand]
    private void NavigateToAddEmployee()
    {
        _navigationService?.NavigateToAddEmployee();
    }

    [RelayCommand]
    private void NavigateToAnimalSearch()
    {
        _navigationService?.NavigateToAnimalSearch();
    }

    [RelayCommand]
    private void NavigateToFamilyPairs()
    {
        _navigationService?.NavigateToFamilyPairs();
    }

    public async Task UpdateDietType(int id)
    {
        await _navigationService?.UpdateDietType(id)!;
    }

    public async Task UpdateDiet(int id)
    {
        await _navigationService?.UpdateDiet(id)!;

    }

    public async Task UpdateEmployee(int id)
    {
        await _navigationService?.UpdateEmployee(id)!;
    }

    public async Task UpdateAnimal(int id)
    {
        await _navigationService?.UpdateAnimal(id)!;
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