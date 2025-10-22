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
    [NotifyPropertyChangedFor(nameof(Animal))]
    private ObservableCollection<BirdsWinterPlace?> _winterPlaces = [BirdsWinterPlace.Empty()];

    [ObservableProperty] 
    private ObservableCollection<ReptileInfo?> _reptileInfos = [ReptileInfo.Empty()];

    [ObservableProperty] 
    private ObservableCollection<Diet?> _diets = [Diet.Empty()];

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

    [ObservableProperty] 
    private string? _error;

    public ObservableCollection<string> SexOptions { get; } = ["Male", "Female", "Unknown"];

    public AnimalEditViewModel(INavigationService navigationService, ISelectService dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await Task.WhenAll(LoadAnimalTypes(),
            LoadBirdsWinterPlaces(),
            LoadReptileInfos(),
            LoadDiets(),
            LoadHabitatZones(),
            LoadCaretakers());
    }

    private async Task LoadAnimalTypes()
    {
        try
        {
            var types = await _dataService.SelectAll<AnimalType>("AnimalTypes");
            
            if (types is null) return;
            foreach (var type in types)
            {
                AnimalTypes.Add(type);
            }
        }
        catch (Exception ex)
        {
            Error += ex.Message;
        }
    }
    
    private async Task LoadBirdsWinterPlaces()
    {
        try
        {
            var places = await _dataService.SelectAll<BirdsWinterPlace>("BirdsWinterPlaces");
            
            if (places is null) return;
            foreach (var place in places)
            {
                WinterPlaces.Add(place);
            }
        }
        catch (Exception ex)
        {
            Error += ex.Message;
        }
    }
    
    private async Task LoadReptileInfos()
    {
        try
        {
            var info = await _dataService.SelectAll<ReptileInfo>("ReptilesInfo");
            
            if (info is null) return;
            foreach (var reptileInfo in info)
            {
                ReptileInfos.Add(reptileInfo);
            }
        }
        catch (Exception ex)
        {
            Error += ex.Message;
        }
    }
    
    private async Task LoadDiets()
    {
        try
        {
            var diets = await _dataService.SelectAll<Diet>("Diets");
            
            if (diets is null) return;
            foreach (var diet in diets)
            {
                Diets.Add(diet);
            }
        }
        catch (Exception ex)
        {
            Error += ex.Message;
        }
    }
    
    private async Task LoadHabitatZones()
    {
        try
        {
            var habitats = await _dataService.SelectAll<HabitatZone>("HabitatZones");
            
            if (habitats is null) return;
            foreach (var habitat in habitats)
            {
                HabitatZones.Add(habitat);
            }
        }
        catch (Exception ex)
        {
            Error += ex.Message;
        }
    }
    
    private async Task LoadCaretakers()
    {
        try
        {
            var caretakers = await _dataService.SelectAll<Employee>("Employees");
            
            if (caretakers is null) return;
            foreach (var caretaker in caretakers)
            {
                Caretakers.Add(caretaker);
            }
        }
        catch (Exception ex)
        {
            Error += ex.Message;
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