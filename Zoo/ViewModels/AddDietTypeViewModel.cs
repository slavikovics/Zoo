using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;
using DatabaseUtils.Repositories;

namespace Zoo.ViewModels;

public partial class AddDietTypeViewModel : ViewModelBase
{
    private readonly ISelectService _dataService;
    
    private readonly IDietTypesRepository _dietTypesRepository;
    
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty] private string _title = "Добавить тип рациона питания";

    [ObservableProperty] private DietType _dietType = new(1, "Новый тип рациона");
    
    public AddDietTypeViewModel(ISelectService dataService, IDietTypesRepository dietTypesRepository, MainViewModel mainViewModel)
    {
        _dataService = dataService;
        _mainViewModel = mainViewModel;
        _dietTypesRepository = dietTypesRepository;
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            await _dietTypesRepository.Create(DietType);
            _mainViewModel.NavigateToDietTypesCommand.Execute(null);
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error adding diet type: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _mainViewModel.NavigateToDietTypesCommand.Execute(null);
    }
}