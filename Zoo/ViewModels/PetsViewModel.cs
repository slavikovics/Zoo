using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class PetsViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;
    private readonly IDeleteService _deleteService;
    private readonly INavigationService _navigationService;

    public ObservableCollection<Animal> Animals { get; set; }

    public PetsViewModel(ISelectService selectService, IDeleteService deleteService,
        INavigationService navigationService)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        _navigationService = navigationService;
        Animals = new ObservableCollection<Animal>();
        Task.Run(LoadAnimals);
    }

    private async Task LoadAnimals()
    {
        try
        {
            var animals = await _selectService.SelectAll<Animal>();
            var animalsList = animals?.ToList();

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Animals.Clear();
                if (animalsList != null)
                {
                    foreach (var animal in animalsList)
                    {
                        Animals.Add(animal);
                    }
                }
            });
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error loading animals: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private async Task DeleteAnimal(int id)
    {
        try
        {
            await _deleteService.Delete<Animal>(id);

            var itemsToRemove = Animals.Where(x => x.Id == id).ToList();
            foreach (var item in itemsToRemove)
            {
                Animals.Remove(item);
            }
        }
        catch (Exception sqlException)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error deleting animals: {sqlException.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private async Task UpdateAnimal(int id)
    {
        try
        {
            await _navigationService.UpdateAnimal(id);
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error updating animals: {e.Message}";
            _ = DelayVisibility();
        }
    }
}