using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;
using DatabaseUtils.Repositories;

namespace Zoo.ViewModels;

public partial class AddDietViewModel : ViewModelBase
{
    private readonly ISelectService _dataService;

    private readonly IDietsRepository _dietsRepository;
    
    private readonly INavigationService _navigationService;
    
    [ObservableProperty] private string _title = "Добавить рацион питания";

    [ObservableProperty] private Diet _diet = new(1, "Новый рацион", 1, null);

    [ObservableProperty] private int _selectedDietType = -1;

    [ObservableProperty] private ObservableCollection<DietType> _dietTypes = [];

    public AddDietViewModel(ISelectService dataService, IDietsRepository dietsRepository, INavigationService navigationService)
    {
        _dataService = dataService;
        _dietsRepository = dietsRepository;
        _navigationService = navigationService;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        IEnumerable<DietType>? items = null;
        try
        {
            items = await _dataService.SelectAll<DietType>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (items is null) return;

        foreach (var item in items.ToList())
        {
            DietTypes.Add(item);
        }

        if (DietTypes.Any()) SelectedDietType = 0;
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            int selectedDietType = DietTypes.ElementAtOrDefault(SelectedDietType)!.Id;
            Diet.TypeId = selectedDietType;
            await _dietsRepository.Create(Diet);
            _navigationService.NavigateToDiets();
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error adding diet: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateToDiets();
    }
}