using System.Threading.Tasks;
using Zoo.ViewModels;

namespace Zoo;

public interface INavigationService
{
    TViewModel NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
    void SetMainViewModel(MainViewModel mainViewModel);
    void EnableAll();
    void NavigateToPets();
    void NavigateToAddPet();
    void NavigateToDiets();
    void NavigateToAddDiet();
    void NavigateToDietTypes();
    void NavigateToAddDietType();
    void NavigateToEmployees();
    void NavigateToAddEmployee();
    void NavigateToAnimalSearch();
    void NavigateToFamilyPairs();
    Task UpdateDietType(int id);
    Task UpdateDiet(int id);
    Task UpdateEmployee(int id);
    Task UpdateAnimal(int id);
    Task DetailedInfo(int id);
}