using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class AddAnimalViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    
    private readonly ISelectService _dataService;
    
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty] private string _title = "Добавить питомца";
    
    [ObservableProperty] private string _animalName = "Новый питомец";

    [ObservableProperty] private int _animalTypeId = -1;
    
    [ObservableProperty] private DateTimeOffset _animalBirthDate = DateTimeOffset.Now;
    
    [ObservableProperty] private string _animalSex = "Unknown";

    [ObservableProperty] private int _animalHabitatZoneId = -1;
    
    [ObservableProperty] private int _animalDietId = -1;
    
    [ObservableProperty] private int _animalCaretakerId = -1;

    [ObservableProperty] private int _animalWinterPlaceId = -1;
    
    [ObservableProperty] private int _animalReptileInfoId = -1;
    
    [ObservableProperty] private ObservableCollection<AnimalType> _animalTypes = [];

    [ObservableProperty] private ObservableCollection<BirdsWinterPlace> _winterPlaces = [BirdsWinterPlace.Empty()];

    [ObservableProperty] private ObservableCollection<ReptileInfo> _reptileInfos = [ReptileInfo.Empty()];

    [ObservableProperty] private ObservableCollection<Diet> _diets = [Diet.Empty()];

    [ObservableProperty] private ObservableCollection<HabitatZone> _habitatZones = [];

    [ObservableProperty] private ObservableCollection<Employee> _caretakers = [];

    [ObservableProperty] private ObservableCollection<Employee> _availableVets = [];

    [ObservableProperty] private ObservableCollection<Employee> _selectedVets = [];
    
    [ObservableProperty] private ObservableCollection<Employee> _vetsToRemove = [];

    [ObservableProperty] private Employee? _selectedVetToAdd;

    [ObservableProperty] private string _error = "";

    public ObservableCollection<string> SexOptions { get; } = ["Male", "Female", "Unknown"];

    public AddAnimalViewModel(INavigationService navigationService, ISelectService dataService, MainViewModel mainViewModel)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _mainViewModel = mainViewModel;
        _ = InitializeAsync();
    }

    private void SetDefaultValues()
    {
        AnimalTypeId = 0;
        AnimalCaretakerId = 0;
        AnimalDietId = 0;
        AnimalHabitatZoneId = 0;
        AnimalReptileInfoId = 0;
        AnimalWinterPlaceId = 0;
    }

    private async Task InitializeAsync()
    {
        await Task.WhenAll(
            LoadDropdown("AnimalTypes", AnimalTypes),
            LoadDropdown("BirdsWinterPlaces", WinterPlaces),
            LoadDropdown("ReptilesInfo", ReptileInfos),
            LoadDropdown("Diets", Diets),
            LoadDropdown("HabitatZones", HabitatZones),
            LoadDropdown("Employees", Caretakers));

        AvailableVets = Caretakers;
        SetDefaultValues();
    }

    private async Task LoadDropdown<T>(string tableName, ObservableCollection<T> targetCollection) where T: class
    {
        try
        {
            var items = await _dataService.SelectAll<T>(tableName);
            
            if (items is null) return;
            foreach (var item in items)
            {
                targetCollection.Add(item);
            }
        }
        catch (Exception e)
        {
            Error += e.Message;
        }
    }

    public void SetAnimal(Animal animal, List<Employee> currentVets)
    {
    }

    [RelayCommand]
    private void AddVet(Employee newVet)
    {
        if (!SelectedVets.Contains(newVet)) SelectedVets.Add(newVet);
    }

    [RelayCommand]
    private void RemoveVet()
    {
        var vetsToRemoveList = VetsToRemove.ToList();
        
        foreach (var vet in vetsToRemoveList)
        {
            SelectedVets.Remove(vet);
        }
    }

    [RelayCommand]
    private async Task Save()
    {
    }

    [RelayCommand]
    private void Cancel()
    {
        _mainViewModel.NavigateToPetsCommand.Execute(null);
    }
}