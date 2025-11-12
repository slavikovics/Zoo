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

public partial class AddDietTypeViewModel : ViewModelBase
{
    private readonly ISelectService _dataService;
    
    private readonly IDietsRepository _dietsRepository;
    
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty] private string _title = "Добавить тип рациона питания";

    [ObservableProperty] private DietType _diet = new(1, "Новый тип рациона");
    
    public AddDietTypeViewModel(ISelectService dataService, IDietsRepository dietsRepository, MainViewModel mainViewModel)
    {
        _dataService = dataService;
        _mainViewModel = mainViewModel;
        _dietsRepository = dietsRepository;
    }

    [RelayCommand]
    private async Task Save()
    {
        //await _dietsRepository.Create(Diet);
        _mainViewModel.NavigateToDietTypesCommand.Execute(null);
    }

    [RelayCommand]
    private void Cancel()
    {
        _mainViewModel.NavigateToDietTypesCommand.Execute(null);
    }
}