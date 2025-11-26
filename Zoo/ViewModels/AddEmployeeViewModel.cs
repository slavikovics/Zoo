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

public partial class AddEmployeeViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly ISelectService _dataService;
    private readonly IEmployeesRepository _employeesRepository;

    private readonly Employee _emptySpouse;

    [ObservableProperty] private string _title = "Добавить сотрудника";
    [ObservableProperty] private Employee _employee = new(1, "Новый сотрудник", DateTime.Now, "+375-25-456-78-89", "Single");
    [ObservableProperty] private ObservableCollection<Employee> _availableSpouses;
    [ObservableProperty] private Employee _selectedSpouse;

    public ObservableCollection<string> MaritalStatusOptions { get; } = ["Single", "Married", "Divorced", "Widowed"];

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsMarried))]
    private string _selectedMaritalStatus = "Single";

    public bool IsMarried => CheckMarried();

    private bool CheckMarried()
    {
        if (SelectedMaritalStatus != "Married")
        {
            SelectedSpouse = _emptySpouse;
            return false;
        }

        return true;
    }

    public AddEmployeeViewModel(INavigationService navigationService, ISelectService dataService,
        IEmployeesRepository employeesRepository)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _employeesRepository = employeesRepository;
        _emptySpouse = Employee.Empty();
        _availableSpouses = [_emptySpouse];
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        IEnumerable<Employee>? items = null;
        try
        {
            items = await _dataService.SelectAll<Employee>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (items is null) return;

        foreach (var item in items.ToList())
        {
            AvailableSpouses.Add(item);
        }

        if (AvailableSpouses.Any()) SelectedSpouse = AvailableSpouses[0];
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            Employee.MaritalStatus = SelectedMaritalStatus;
            Employee.Spouse = SelectedSpouse;
            await _employeesRepository.Create(Employee);
            _navigationService.NavigateToEmployees();
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error adding employee: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateToEmployees();
    }
}