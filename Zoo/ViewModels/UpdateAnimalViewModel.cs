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

public partial class UpdateAnimalViewModel : ViewModelBase
{
    private readonly ISelectService _dataService;
    private readonly IAnimalsRepository _animalsRepository;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private string _title = "Редактировать питомца";
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
    [ObservableProperty] private string _error = "";
    [ObservableProperty] private Animal _animal;
    public ObservableCollection<string> SexOptions { get; } = ["Male", "Female", "Unknown"];

    public UpdateAnimalViewModel(ISelectService dataService, IAnimalsRepository animalsRepository,
        INavigationService navigationService)
    {
        _dataService = dataService;
        _animalsRepository = animalsRepository;
        _navigationService = navigationService;
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

    public async Task InitializeAsync(int id)
    {
        await Task.WhenAll(
            LoadDropdown(AnimalTypes),
            LoadDropdown(WinterPlaces),
            LoadDropdown(ReptileInfos),
            LoadDropdown(Diets),
            LoadDropdown(HabitatZones),
            LoadDropdown(Caretakers));

        var animals = await _animalsRepository.SelectById(id);
        Animal = animals?.FirstOrDefault()!;

        AvailableVets = Caretakers;
        SetDefaultValues();
        PopulateFromAnimal();

        var vets = await _animalsRepository.GetAllVets(id);
        foreach (var vet in vets)
        {
            SelectedVets.Add(vet);
        }
    }

    private void PopulateFromAnimal()
    {
        AnimalName = Animal.Name ?? string.Empty;
        AnimalSex = string.IsNullOrWhiteSpace(Animal.Sex) ? "Unknown" : Animal.Sex;
        AnimalBirthDate = new DateTimeOffset((DateTime)Animal.BirthDate!);

        AnimalTypeId = AnimalTypes
            .Select((t, i) => new { t, i })
            .FirstOrDefault(x => x.t.Id == Animal.TypeId)?.i ?? 0;

        AnimalDietId = Diets
            .Select((d, i) => new { d, i })
            .FirstOrDefault(x => x.d.Id == Animal.DietId)?.i ?? 0;

        AnimalHabitatZoneId = HabitatZones
            .Select((h, i) => new { h, i })
            .FirstOrDefault(x => x.h.Id == Animal.HabitatZoneId)?.i ?? 0;

        AnimalCaretakerId = Caretakers
            .Select((c, i) => new { c, i })
            .FirstOrDefault(x => x.c.Id == Animal.CaretakerId)?.i ?? 0;

        AnimalWinterPlaceId = WinterPlaces
            .Select((w, i) => new { w, i })
            .FirstOrDefault(x => x.w.Id == Animal.WinterPlaceId)?.i ?? 0;

        AnimalReptileInfoId = ReptileInfos
            .Select((r, i) => new { r, i })
            .FirstOrDefault(x => x.r.Id == Animal.ReptileInfoId)?.i ?? 0;
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
            ErrorMessage = $"Error loading dropdown: {e.Message}";
            _ = DelayVisibility();
        }
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
        await _animalsRepository.RemoveAllVets(Animal.Id);
        int? reptileInfoId = ReptileInfos.ElementAtOrDefault(AnimalReptileInfoId)!.Id;
        int? winterPlaceId = WinterPlaces.ElementAtOrDefault(AnimalWinterPlaceId)!.Id;

        Animal animal = new(
            Animal.Id,
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

        try
        {
            await _animalsRepository.Update(animal);
            await _animalsRepository.AddVets(animal.Id, SelectedVets.ToList());

            _navigationService.NavigateToPets();
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error updating animal: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateToPets();
    }
}