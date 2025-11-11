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
    private readonly INavigationService _navigationService;
    
    private readonly ISelectService _dataService;
    
    private readonly IDietsRepository _dietsRepository;
    
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty] private string _title = "Добавить рацион питания";

    [ObservableProperty] private Diet _diet = new(1, "Новый рацион", 1, null);
    
    [ObservableProperty] private int _selectedDietType = -1;

    [ObservableProperty] private ObservableCollection<DietType> _dietTypes = [];

    public AddDietViewModel(INavigationService navigationService, ISelectService dataService, IDietsRepository dietsRepository, MainViewModel mainViewModel)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _mainViewModel = mainViewModel;
        _dietsRepository = dietsRepository;
        InitializeAsync();
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
        int selectedDietType = DietTypes.ElementAtOrDefault(SelectedDietType)!.Id;
        Diet.TypeId = selectedDietType;
        await _dietsRepository.Create(Diet);
        
        _mainViewModel.NavigateToDietsCommand.Execute(null);
    }

    [RelayCommand]
    private void Cancel()
    {
        _mainViewModel.NavigateToDietsCommand.Execute(null);
    }
}