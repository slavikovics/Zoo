using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class PetsViewModel : ViewModelBase
{
    private readonly ISelectService<Animal> _selectService;

    public ObservableCollection<Animal> Animals { get; set; }
    
    public PetsViewModel(ISelectService<Animal> selectService)
    {
        _selectService = selectService;
        Animals = new ObservableCollection<Animal>();
        Task.Run(LoadAnimals);
    }

    private async Task LoadAnimals()
    {
        try
        {
            var animals = await _selectService.SelectAll("Animals");
            var animalsList = animals?.ToList();

            // Обновляем коллекцию в UI потоке
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

            Console.WriteLine($"Loaded {Animals.Count} animals"); // Для отладки
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading animals: {ex.Message}");
            // Можно добавить обработку ошибок
        }
    }
}