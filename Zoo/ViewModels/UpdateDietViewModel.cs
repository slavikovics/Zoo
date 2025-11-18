using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;
using DatabaseUtils.Repositories;

namespace Zoo.ViewModels;

public partial class UpdateDietViewModel : ViewModelBase
{
    private readonly ISelectService _dataService;
    private readonly IDietsRepository _dietsRepository;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private string _title = "Редактировать рацион питания";
    [ObservableProperty] private Diet _diet;
    [ObservableProperty] private int _selectedDietType = -1;
    [ObservableProperty] private ObservableCollection<DietType> _dietTypes = [];

    public UpdateDietViewModel(ISelectService dataService, IDietsRepository dietsRepository,
        INavigationService navigationService)
    {
        _dataService = dataService;
        _dietsRepository = dietsRepository;
        _navigationService = navigationService;
    }

    public async Task InitializeAsync(int id)
    {
        IEnumerable<DietType>? items = null;
        try
        {
            items = await _dataService.SelectAll<DietType>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (items is null) return;

        foreach (var item in items.ToList())
        {
            DietTypes.Add(item);
        }

        var diets = await _dietsRepository.SelectById(id);
        if (diets is not null)
        {
            Diet = diets.First();
        }

        SelectedDietType = DietTypes
            .Select((dt, index) => new { dt, index })
            .Where(x => x.dt.Id == Diet.TypeId)
            .Select(x => x.index)
            .FirstOrDefault();
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            int selectedDietType = DietTypes.ElementAtOrDefault(SelectedDietType)!.Id;
            Diet.TypeId = selectedDietType;
            await _dietsRepository.Update(Diet);

            _navigationService.NavigateToDiets();
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error updating diet: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateToDiets();
    }
}