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
    private readonly IDietTypesRepository _dietTypesRepository;

    private readonly INavigationService _navigationService;

    [ObservableProperty] private string _title = "Добавить тип рациона питания";

    [ObservableProperty] private DietType _dietType = new(1, "Новый тип рациона");

    public AddDietTypeViewModel(IDietTypesRepository dietTypesRepository, INavigationService navigationService)
    {
        _dietTypesRepository = dietTypesRepository;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            await _dietTypesRepository.Create(DietType);
            _navigationService.NavigateToDietTypes();
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
        _navigationService.NavigateToDietTypes();
    }
}