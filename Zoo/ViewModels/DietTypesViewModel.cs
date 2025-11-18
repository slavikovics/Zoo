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

    private readonly MainViewModel _mainViewModel;

    public ObservableCollection<DietType> DietTypes { get; set; }

    public DietTypesViewModel(ISelectService selectService, IDeleteService deleteService, MainViewModel mainViewModel)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        _mainViewModel = mainViewModel;
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
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error loading diet types: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private async Task DeleteDietTypes(int id)
    {
        try
        {
            await _deleteService.Delete<DietType>(id);

            var itemsToRemove = DietTypes.Where(x => x.Id == id).ToList();
            foreach (var item in itemsToRemove)
            {
                DietTypes.Remove(item);
            }
        }
        catch (Exception sqlException)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error deleting diet type: {sqlException.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private async Task UpdateDietTypes(int id)
    {
        try
        {
            await _mainViewModel.UpdateDietType(id);
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error updating diet type: {e.Message}";
            _ = DelayVisibility();
        }
    }
}