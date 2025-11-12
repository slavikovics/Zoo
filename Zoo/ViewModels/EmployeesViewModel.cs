using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class EmployeesViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;
    
    private readonly IDeleteService _deleteService;
    
    private readonly MainViewModel _mainViewModel;

    public ObservableCollection<Employee> Employees { get; set; }
    
    public EmployeesViewModel(ISelectService selectService, IDeleteService deleteService, MainViewModel mainViewModel)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        _mainViewModel = mainViewModel;
        Employees = new ObservableCollection<Employee>();
        Task.Run(LoadEmployees);
    }

    private async Task LoadEmployees()
    {
        try
        {
            var employees = await _selectService.SelectAll<Employee>();
            var employeesList = employees?.ToList();
            
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Employees.Clear();
                if (employeesList != null)
                {
                    foreach (var diet in employeesList)
                    {
                        Employees.Add(diet);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading employees: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DeleteEmployee(int id)
    {
        try
        {
            await _deleteService.Delete<Employee>(id);
            
            var itemsToRemove = Employees.Where(x => x.Id == id).ToList();
            foreach (var item in itemsToRemove)
            {
                Employees.Remove(item);
            }
        }
        catch (Exception sqlException)
        {
            Console.WriteLine(sqlException.Message);
        }
    }
    
    [RelayCommand]
    private async Task UpdateEmployee(int id)
    {
        await _mainViewModel.UpdateEmployee(id);
    }
}