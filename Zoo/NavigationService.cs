using System;
using System.Threading.Tasks;
using Zoo.ViewModels;

namespace Zoo;

public class NavigationService : INavigationService
{
    private MainViewModel _mainViewModel;

    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void SetMainViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    public void EnableAll()
    {
        _mainViewModel.IsSearchEnabled = true;
        _mainViewModel.IsBackEnabled = true;
        _mainViewModel.IsAddEnabled = true;
        _mainViewModel.AreAnimalsEnabled = true;
        _mainViewModel.AreDietTypesEnabled = true;
        _mainViewModel.AreDietsEnabled = true;
        _mainViewModel.AreEmployeesEnabled = true;
        _mainViewModel.IsSearchEnabled = true;
        _mainViewModel.AreFamilyPairsEnabled = true;
    }

    public void NavigateToPets()
    {
        EnableAll();
        _mainViewModel.AreAnimalsEnabled = false;
        _mainViewModel.IsBackEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<PetsViewModel>();
    }

    public void NavigateToAddPet()
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<AddAnimalViewModel>();
    }

    public void NavigateToDiets()
    {
        EnableAll();
        _mainViewModel.IsBackEnabled = false;
        _mainViewModel.AreDietsEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<DietsViewModel>();
    }

    public void NavigateToAddDiet()
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<AddDietViewModel>();
    }

    public void NavigateToDietTypes()
    {
        EnableAll();
        _mainViewModel.IsBackEnabled = false;
        _mainViewModel.AreDietTypesEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<DietTypesViewModel>();
    }

    public void NavigateToAddDietType()
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<AddDietTypeViewModel>();
    }

    public void NavigateToEmployees()
    {
        EnableAll();
        _mainViewModel.IsBackEnabled = false;
        _mainViewModel.AreEmployeesEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<EmployeesViewModel>();
    }

    public void NavigateToAddEmployee()
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<AddEmployeeViewModel>();
    }

    public void NavigateToAnimalSearch()
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.IsBackEnabled = false;
        _mainViewModel.IsSearchEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<PetsSearchViewModel>();
    }

    public void NavigateToFamilyPairs()
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.IsBackEnabled = false;
        _mainViewModel.AreFamilyPairsEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<FamilyPairsViewModel>();
    }

    public async Task UpdateDietType(int id)
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<UpdateDietTypeViewModel>();
        await (_mainViewModel.CurrentViewModel as UpdateDietTypeViewModel)!.InitializeAsync(id);
    }

    public async Task UpdateDiet(int id)
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<UpdateDietViewModel>();
        await (_mainViewModel.CurrentViewModel as UpdateDietViewModel)!.InitializeAsync(id);
    }

    public async Task UpdateEmployee(int id)
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<UpdateEmployeeViewModel>();
        await (_mainViewModel.CurrentViewModel as UpdateEmployeeViewModel)!.InitializeAsync(id);
    }

    public async Task UpdateAnimal(int id)
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<UpdateAnimalViewModel>();
        await (_mainViewModel.CurrentViewModel as UpdateAnimalViewModel)!.InitializeAsync(id);
    }

    public async Task DetailedInfo(int id)
    {
        EnableAll();
        _mainViewModel.IsAddEnabled = false;
        _mainViewModel.CurrentViewModel = NavigateTo<AnimalDetailsViewModel>();
        await (_mainViewModel.CurrentViewModel as AnimalDetailsViewModel)!.InitializeAsync(id);
    }

    public TViewModel NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        var viewModel = _serviceProvider.GetService(typeof(TViewModel)) as TViewModel;

        _mainViewModel.CurrentViewModel = viewModel ?? throw new InvalidOperationException(
            $"ViewModel {typeof(TViewModel).Name} is not registered in the service container.");
        return viewModel;
    }
}