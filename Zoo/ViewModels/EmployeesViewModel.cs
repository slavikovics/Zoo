using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public class EmployeesViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;

    public ObservableCollection<Employee> Employees { get; set; }
    
    public EmployeesViewModel(ISelectService selectService)
    {
        _selectService = selectService;
        Employees = new ObservableCollection<Employee>();
        Task.Run(LoadAnimals);
    }

    private async Task LoadAnimals()
    {
        try
        {
            var diets = await _selectService.SelectAll<Employee>("Employees");
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
}