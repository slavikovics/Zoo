using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;
using DatabaseUtils.Repositories;

namespace Zoo.ViewModels;

public partial class AddAnimalViewModel : ViewModelBase
{
    private readonly ISelectService _dataService;
    private readonly IAnimalsRepository _animalsRepository;
    private readonly INavigationService _navigationService;

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
    [ObservableProperty] private ObservableCollection<Diet> _diets = [];
    [ObservableProperty] private ObservableCollection<HabitatZone> _habitatZones = [];
    [ObservableProperty] private ObservableCollection<Employee> _caretakers = [];
    [ObservableProperty] private ObservableCollection<Employee> _availableVets = [];
    [ObservableProperty] private ObservableCollection<Employee> _selectedVets = [];
    [ObservableProperty] private ObservableCollection<Employee> _vetsToRemove = [];
    [ObservableProperty] private Employee? _selectedVetToAdd;

    public ObservableCollection<string> SexOptions { get; } = ["Male", "Female", "Unknown"];

    public AddAnimalViewModel(ISelectService dataService, IAnimalsRepository animalsRepository,
        INavigationService navigationService)
    {
        _dataService = dataService;
        _animalsRepository = animalsRepository;
        _navigationService = navigationService;
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
            LoadDropdown(AnimalTypes),
            LoadDropdown(WinterPlaces),
            LoadDropdown(ReptileInfos),
            LoadDropdown(Diets),
            LoadDropdown(HabitatZones),
            LoadDropdown(Caretakers));

        AvailableVets = Caretakers;
        SetDefaultValues();
    }

    private async Task LoadDropdown<T>(ObservableCollection<T> targetCollection) where T : class
    {
        try
        {
            var items = await _dataService.SelectAll<T>();

            if (items is null) return;
            foreach (var item in items)
            {
                targetCollection.Add(item);
            }
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error deleting diet: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private void AddVet(object vet)
    {
        if (vet is not Employee newVet) return;
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
        try
        {
            int? reptileInfoId = ReptileInfos.ElementAtOrDefault(AnimalReptileInfoId)!.Id;
            int? winterPlaceId = WinterPlaces.ElementAtOrDefault(AnimalWinterPlaceId)!.Id;

            Animal animal = new(
                1,
                AnimalName,
                AnimalTypes.ElementAtOrDefault(AnimalTypeId)!.Id,
                AnimalBirthDate.DateTime,
                AnimalSex,
                winterPlaceId,
                reptileInfoId,
                Diets.ElementAtOrDefault(AnimalDietId)!.Id,
                HabitatZones.ElementAtOrDefault(AnimalHabitatZoneId)!.Id,
                (int)Caretakers.ElementAtOrDefault(AnimalCaretakerId)!.Id!
            );

            await _animalsRepository.CreateWithVets(animal, SelectedVets.ToList());
            _navigationService.NavigateToPets();
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error adding animal: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateToPets();
    }
}