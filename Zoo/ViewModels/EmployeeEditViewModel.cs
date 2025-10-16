using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class EmployeeEditViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly ISelectService<Employee> _dataService;

    [ObservableProperty] private string _title = "Create Employee";

    [ObservableProperty] private Employee _employee = new(1, "1", DateTime.MaxValue, "232", "3243", null);

    [ObservableProperty] private ObservableCollection<Employee> _availableSpouses = new();
    
    public ObservableCollection<string> MaritalStatusOptions { get; } = new()
    {
        "Single", "Married", "Divorced", "Widowed"
    };
        
    public bool IsMarried => Employee?.MaritalStatus == "Married";

    public EmployeeEditViewModel(INavigationService navigationService, ISelectService<Employee> dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
    }

    private async void InitializeAsync()
    {
    }

    public void SetEmployee(Employee employee)
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