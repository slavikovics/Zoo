using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class DietEditViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly ISelectService<Diet> _dataService;

    [ObservableProperty] private string _title = "Create Diet";

    [ObservableProperty] private Diet _diet = new(1, "1", 1, null);

    [ObservableProperty] private ObservableCollection<DietType> _dietTypes = new();

    public DietEditViewModel(INavigationService navigationService, ISelectService<Diet> dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
    }

    public void SetDiet(Diet diet)
    {
    }

    [RelayCommand]
    private async void Save()
    {
    }

    [RelayCommand]
    private void Cancel()
    {
    }
}