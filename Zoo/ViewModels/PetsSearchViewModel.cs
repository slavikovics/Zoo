using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;
using DatabaseUtils.Repositories;

namespace Zoo.ViewModels;

public partial class PetsSearchViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;
    private readonly IAnimalsRepository _animalsRepository;

    public ObservableCollection<Animal> Animals { get; set; }
    public ObservableCollection<AnimalType> AnimalTypes { get; set; }

    public string SearchName { get; set; } = "";
    public AnimalType? SelectedType { get; set; }

    public PetsSearchViewModel(
        ISelectService selectService,
        IAnimalsRepository animalsRepository)
    {
        _selectService = selectService;
        _animalsRepository = animalsRepository;

        Animals = new ObservableCollection<Animal>();
        AnimalTypes = new ObservableCollection<AnimalType>();

        Task.Run(LoadTypes);
    }
    
    private async Task LoadTypes()
    {
        try
        {
            var types = await _selectService.SelectAll<AnimalType>();
            var list = types?.ToList();

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                AnimalTypes.Clear();
                if (list != null)
                {
                    foreach (var t in list)
                        AnimalTypes.Add(t);
                }
            });
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error loading animal types: {e.Message}";
            _ = DelayVisibility();
        }
    }
    
    [RelayCommand]
    private async Task Search()
    {
        try
        {
            int? typeId = SelectedType?.Id;
            
            var result = await _animalsRepository.Search(SearchName, typeId);
            var list = result?.ToList();

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Animals.Clear();

                if (list != null)
                {
                    foreach (var animal in list)
                        Animals.Add(animal);
                }
            });
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error searching animals: {e.Message}";
            _ = DelayVisibility();
        }
    }
}