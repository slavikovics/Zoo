using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;
using Microsoft.Data.SqlClient;

namespace Zoo.ViewModels;

public partial class EmployeesViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;
    
    private readonly IDeleteService _deleteService;

    public ObservableCollection<Employee> Employees { get; set; }
    
    public EmployeesViewModel(ISelectService selectService, IDeleteService deleteService)
    {
        _selectService = selectService;
        _deleteService = deleteService;
        Employees = new ObservableCollection<Employee>();
        Task.Run(LoadAnimals);
    }

    private async Task LoadAnimals()
    {
        try
        {
            var diets = await _selectService.SelectAll<Employee>();
            var dietsList = diets?.ToList();
            
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Employees.Clear();
                if (dietsList != null)
                {
                    foreach (var diet in dietsList)
                    {
                        Employees.Add(diet);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading animals: {ex.Message}");
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
}