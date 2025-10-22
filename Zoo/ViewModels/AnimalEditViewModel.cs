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

public partial class AnimalEditViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    
    private readonly ISelectService _dataService;

    [ObservableProperty] 
    private string _title = "Создать питомца";

    [ObservableProperty] 
    private Animal _animal = new(1, "Новый питомец", 1, DateTime.MaxValue, "Unknown", 1, 1, 1, 1, 1, 1);

    [ObservableProperty] 
    private ObservableCollection<AnimalType> _animalTypes = [];

    [ObservableProperty]
    private ObservableCollection<BirdsWinterPlace> _winterPlaces = [BirdsWinterPlace.Empty()];

    [ObservableProperty] 
    private ObservableCollection<ReptileInfo> _reptileInfos = [ReptileInfo.Empty()];

    [ObservableProperty] 
    private ObservableCollection<Diet> _diets = [Diet.Empty()];

    [ObservableProperty] 
    private ObservableCollection<HabitatZone> _habitatZones = [];

    [ObservableProperty] 
    private ObservableCollection<Employee> _caretakers = [];

    [ObservableProperty] 
    private ObservableCollection<Employee> _availableVets = [];

    [ObservableProperty] 
    private ObservableCollection<Employee> _selectedVets = [];
    
    [ObservableProperty]
    private ObservableCollection<Employee> _vetsToRemove = [];

    [ObservableProperty] 
    private Employee? _selectedVetToAdd;

    [ObservableProperty] 
    private string _error = "";

    public ObservableCollection<string> SexOptions { get; } = ["Male", "Female", "Unknown"];

    public AnimalEditViewModel(INavigationService navigationService, ISelectService dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _ = InitializeAsync();
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
    }
}