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
    
    private readonly ISelectService<Animal> _dataService;

    [ObservableProperty] private string _title = "Create Animal";

    [ObservableProperty] private Animal _animal = new(1, "1", 1, DateTime.MaxValue, "Unknown", null, null, 1, 1, 1, 1);

    [ObservableProperty] private ObservableCollection<AnimalType> _animalTypes = new();

    [ObservableProperty] private ObservableCollection<BirdsWinterPlace> _winterPlaces = new();

    [ObservableProperty] private ObservableCollection<ReptileInfo> _reptileInfos = new();

    [ObservableProperty] private ObservableCollection<Diet> _diets = new();

    [ObservableProperty] private ObservableCollection<HabitatZone> _habitatZones = new();

    [ObservableProperty] private ObservableCollection<Employee> _caretakers = new();

    [ObservableProperty] private ObservableCollection<Employee> _availableVets = new();

    [ObservableProperty] private ObservableCollection<Employee> _selectedVets = new();

    [ObservableProperty] private Employee? _selectedVetToAdd;

    public ObservableCollection<string> SexOptions { get; } = ["Male", "Female", "Unknown"];

    public AnimalEditViewModel(INavigationService navigationService, ISelectService<Animal> dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
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