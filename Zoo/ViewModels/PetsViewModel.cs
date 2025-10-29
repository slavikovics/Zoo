using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;
using Zoo.Views;

namespace Zoo.ViewModels;

public partial class PetsViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;
    
    private readonly IDeleteService _deleteService;

    public ObservableCollection<Animal> Animals { get; set; }
    
    public PetsViewModel(ISelectService selectService, IDeleteService deleteService)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        Animals = new ObservableCollection<Animal>();
        Task.Run(LoadAnimals);
    }

    private async Task LoadAnimals()
    {
        try
        {
            var animals = await _selectService.SelectAll<Animal>("Animals");
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading animals: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DeleteAnimal(int id)
    {
        try
        {
            await _deleteService.Delete(id, "Animals", "Id");
            
            var itemsToRemove = Animals.Where(x => x.Id == id).ToList();
            foreach (var item in itemsToRemove)
            {
                Animals.Remove(item);
            }
        }
        catch (Exception sqlException)
        {
            Console.WriteLine(sqlException.Message);
        }
    }
}