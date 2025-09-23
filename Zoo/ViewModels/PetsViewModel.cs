using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.DTOs;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class PetsViewModel : ViewModelBase
{
    private readonly ISelectService<Animal> _selectService;
    
    public ObservableCollection<Animal> Animals { get; set; }
    
    public PetsViewModel(ISelectService<Animal> selectService)
    {
        _selectService = selectService;
        LoadAnimals();
    }

    private async Task LoadAnimals()
    {
        var animals = await _selectService.SelectAll("Animals");
        if (animals != null) Animals = new ObservableCollection<Animal>(animals);
    }
}