using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;
using DatabaseUtils.Repositories;

namespace Zoo.ViewModels;

public partial class UpdateDietTypeViewModel : ViewModelBase
{
    private readonly ISelectService _dataService;
    
    private readonly IDietTypesRepository _dietTypesRepository;
    
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty] private string _title = "Редактировать тип рациона питания";

    [ObservableProperty] private DietType _dietType;
    
    public UpdateDietTypeViewModel(ISelectService dataService, IDietTypesRepository dietTypesRepository, MainViewModel mainViewModel)
    {
        _dataService = dataService;
        _mainViewModel = mainViewModel;
        _dietTypesRepository = dietTypesRepository;
    }

    public async Task InitializeAsync(int id)
    {
        var types = await _dataService.SelectById<DietType>(id);
        if (types is not null)
        {
            DietType = types.ToList()[0];
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        await _dietTypesRepository.Update(DietType);
        _mainViewModel.NavigateToDietTypesCommand.Execute(null);
    }

    [RelayCommand]
    private void Cancel()
    {
        _mainViewModel.NavigateToDietTypesCommand.Execute(null);
    }
}