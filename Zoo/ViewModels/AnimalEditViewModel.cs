using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class AnimalEditViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    
    private readonly ISelectService _dataService;

    [ObservableProperty] 
    private string _title = "Создать питомца";

    [ObservableProperty] 
    private Animal _animal = new(1, "1", 1, DateTime.MaxValue, "Unknown", null, null, 1, 1, 1, 1);

    [ObservableProperty] 
    private ObservableCollection<AnimalType> _animalTypes = [];

    [ObservableProperty] 
    private ObservableCollection<BirdsWinterPlace> _winterPlaces = [];

    [ObservableProperty] 
    private ObservableCollection<ReptileInfo> _reptileInfos = [];

    [ObservableProperty] 
    private ObservableCollection<Diet> _diets = [];

    [ObservableProperty] 
    private ObservableCollection<HabitatZone> _habitatZones = [];

    [ObservableProperty] 
    private ObservableCollection<Employee> _caretakers = [];

    [ObservableProperty] 
    private ObservableCollection<Employee> _availableVets = [];

    [ObservableProperty] 
    private ObservableCollection<Employee> _selectedVets = [];

    [ObservableProperty] 
    private Employee? _selectedVetToAdd;

    public ObservableCollection<string> SexOptions { get; } = ["Male", "Female", "Unknown"];

    public AnimalEditViewModel(INavigationService navigationService, ISelectService dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        var types = await _dataService.SelectAll<AnimalType>("AnimalTypes");
        if (types is null) return;
        foreach (var type in types)
        {
            AnimalTypes.Add(type);
        }
    }

    public void SetAnimal(Animal animal, List<Employee> currentVets)
    {
    }

    [RelayCommand]
    private void AddVet()
    {
    }

    [RelayCommand]
    private void RemoveVet()
    {
    }

    [RelayCommand]
    private async Task Save()
    {
    }

    [RelayCommand]
    private void Cancel()
    {
    }
}