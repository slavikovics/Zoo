using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class DietTypesViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;

    private readonly IDeleteService _deleteService;

    public ObservableCollection<DietType> DietTypes { get; set; }
    
    public DietTypesViewModel(ISelectService selectService, IDeleteService deleteService)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        DietTypes = new ObservableCollection<DietType>();
        Task.Run(LoadDietTypes);
    }

    private async Task LoadDietTypes()
    {
        try
        {
            var dietTypes = await _selectService.SelectAll<DietType>();
            var dietTypesList = dietTypes?.ToList();
            
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                DietTypes.Clear();
                if (dietTypesList != null)
                {
                    foreach (var diet in dietTypesList)
                    {
                        DietTypes.Add(diet);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading diet types: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DeleteDietTypes(int id)
    {
        try
        {
            await _deleteService.Delete<Diet>(id);
            
            var itemsToRemove = DietTypes.Where(x => x.Id == id).ToList();
            foreach (var item in itemsToRemove)
            {
                DietTypes.Remove(item);
            }
        }
        catch (Exception sqlException)
        {
            Console.WriteLine(sqlException.Message);
        }
    }
}