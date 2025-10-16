using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public class DietsViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;

    public ObservableCollection<Diet> Diets { get; set; }
    
    public DietsViewModel(ISelectService selectService)
    {
        _selectService = selectService;
        Diets = new ObservableCollection<Diet>();
        Task.Run(LoadAnimals);
    }

    private async Task LoadAnimals()
    {
        try
        {
            var diets = await _selectService.SelectAll<Diet>("Diets");
            var dietsList = diets?.ToList();
            
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Diets.Clear();
                if (dietsList != null)
                {
                    foreach (var diet in dietsList)
                    {
                        Diets.Add(diet);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading animals: {ex.Message}");
        }
    }
}