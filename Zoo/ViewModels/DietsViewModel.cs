using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class DietsViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;

    private readonly IDeleteService _deleteService;

    private readonly MainViewModel _mainViewModel;

    public ObservableCollection<Diet> Diets { get; set; }

    public DietsViewModel(ISelectService selectService, IDeleteService deleteService, MainViewModel mainViewModel)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        _mainViewModel = mainViewModel;
        Diets = new ObservableCollection<Diet>();
        Task.Run(LoadDiets);
    }

    private async Task LoadDiets()
    {
        try
        {
            var diets = await _selectService.SelectAll<Diet>();
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
            IsErrorVisible = true;
            ErrorMessage = $"Error loading diets: {ex.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private async Task DeleteDiet(int id)
    {
        try
        {
            await _deleteService.Delete<Diet>(id);

            var itemsToRemove = Diets.Where(x => x.Id == id).ToList();
            foreach (var item in itemsToRemove)
            {
                Diets.Remove(item);
            }
        }
        catch (Exception sqlException)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error deleting diet: {sqlException.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private async Task UpdateDiet(int id)
    {
        try
        {
            await _mainViewModel.UpdateDiet(id);
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error updating diet: {e.Message}";
            _ = DelayVisibility();
        }
    }
}