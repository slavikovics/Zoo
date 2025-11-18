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

public partial class AnimalDetailsViewModel : ViewModelBase
{
    private readonly ISelectService _dataService;
    private readonly INavigationService _navigationService;
    private readonly IAnimalsRepository _animalsRepository;

    [ObservableProperty] private string _title = "Детальная информация о питомце";
    [ObservableProperty] private Animal _animal;
    [ObservableProperty] private AnimalType _animalType;
    [ObservableProperty] private HabitatZone _habitatZone;
    [ObservableProperty] private Diet _diet;
    [ObservableProperty] private Employee _caretaker;
    [ObservableProperty] private BirdsWinterPlace _winterPlace;
    [ObservableProperty] private ReptileInfo _reptileInfo;
    [ObservableProperty] private ObservableCollection<Employee> _vets = [];

    public AnimalDetailsViewModel(ISelectService dataService, INavigationService navigationService,
        IAnimalsRepository animalsRepository)
    {
        _dataService = dataService;
        _navigationService = navigationService;
        _animalsRepository = animalsRepository;
    }

    public async Task InitializeAsync(int animalId)
    {
        try
        {
            var animals = await _dataService.SelectById<Animal>(animalId);
            Animal = animals?.FirstOrDefault()!;

            await Task.WhenAll(
                LoadAnimalType(),
                LoadHabitatZone(),
                LoadDiet(),
                LoadCaretaker(),
                LoadWinterPlace(),
                LoadReptileInfo(),
                LoadVets()
            );
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error loading animal details: {e.Message}";
            _ = DelayVisibility();
        }
    }

    private async Task LoadAnimalType()
    {
        var animalTypes = await _dataService.SelectById<AnimalType>(Animal.TypeId);
        AnimalType = animalTypes?.FirstOrDefault()!;
    }

    private async Task LoadHabitatZone()
    {
        var habitatZones = await _dataService.SelectById<HabitatZone>(Animal.HabitatZoneId);
        HabitatZone = habitatZones?.FirstOrDefault()!;
    }

    private async Task LoadDiet()
    {
        var diets = await _dataService.SelectById<Diet>(Animal.DietId);
        Diet = diets?.FirstOrDefault()!;
    }

    private async Task LoadCaretaker()
    {
        var caretakers = await _dataService.SelectById<Employee>(Animal.CaretakerId);
        Caretaker = caretakers?.FirstOrDefault()!;
    }

    private async Task LoadWinterPlace()
    {
        if (Animal.WinterPlaceId.HasValue)
        {
            var id = (int)Animal.WinterPlaceId;
            var winterPlaces = await _dataService.SelectById<BirdsWinterPlace>(id);
            WinterPlace = winterPlaces?.FirstOrDefault()!;
        }
        else
        {
            WinterPlace = BirdsWinterPlace.Empty();
        }
    }

    private async Task LoadReptileInfo()
    {
        if (Animal.ReptileInfoId.HasValue)
        {
            var id = (int)Animal.ReptileInfoId;
            var reptileInfos = await _dataService.SelectById<ReptileInfo>(id);
            ReptileInfo = reptileInfos?.FirstOrDefault()!;
        }
        else
        {
            ReptileInfo = ReptileInfo.Empty();
        }
    }

    private async Task LoadVets()
    {
        var allVets = await _animalsRepository.GetAllVets(Animal.Id);
        Vets = new ObservableCollection<Employee>();
        foreach (var vet in allVets)
        {
            Vets.Add(vet);
        }
    }

    [RelayCommand]
    private void Back()
    {
        _navigationService.NavigateToPets();
    }
}