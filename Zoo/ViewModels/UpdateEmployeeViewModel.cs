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

public partial class UpdateEmployeeViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly ISelectService _dataService;
    private readonly IEmployeesRepository _employeesRepository;
    
    private readonly Employee _emptySpouse;

    [ObservableProperty] private string _title = "Редактировать сотрудника";
    [ObservableProperty] private Employee _employee;
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
    

    public UpdateEmployeeViewModel(INavigationService navigationService, ISelectService dataService,
        IEmployeesRepository employeesRepository)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        _employeesRepository = employeesRepository;
        
        _emptySpouse = Employee.Empty();
        _availableSpouses = [_emptySpouse];
    }

    public async Task InitializeAsync(int id)
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

        var employees = await _employeesRepository.SelectById(id);

        if (employees is null) return;
        Employee = employees.First();
        SelectedMaritalStatus = Employee.MaritalStatus;

        if (Employee.MaritalStatus == "Married")
        {
            var spouseId = await _employeesRepository.GetSpouseId((int)Employee.Id!);

            if (spouseId is not null)
            {
                var index = AvailableSpouses
                    .Select((e, i) => new { e, i })
                    .FirstOrDefault(x => x.e.Id == spouseId)?.i ?? -1;

                if (index >= 0)
                    SelectedSpouse = AvailableSpouses[index];
                else
                    SelectedSpouse = Employee.Empty();
            }
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            Employee.MaritalStatus = SelectedMaritalStatus;
            Employee.Spouse = SelectedSpouse;
            await _employeesRepository.Update(Employee);
            _navigationService.NavigateToEmployees();
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error updating employee: {e.Message}";
            _ = DelayVisibility();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateToEmployees();
    }
}