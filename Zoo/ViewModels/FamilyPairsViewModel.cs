using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DatabaseUtils.Models;
using DatabaseUtils.Queries;

namespace Zoo.ViewModels;

public partial class FamilyPairsViewModel : ViewModelBase
{
    private readonly ISelectService _selectService;

    public ObservableCollection<FamilyPair> FamilyPairs { get; set; }

    public FamilyPairsViewModel(ISelectService selectService)
    {
        _selectService = selectService;
        FamilyPairs = new ObservableCollection<FamilyPair>();
        Task.Run(LoadFamilyPairs);
    }

    private async Task LoadFamilyPairs()
    {
        try
        {
            var familyPairs = await _selectService.SelectAll<FamilyPair>();
            var pairs = familyPairs?.ToList();

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                FamilyPairs.Clear();
                if (pairs != null)
                {
                    foreach (var pair in pairs)
                    {
                        FamilyPairs.Add(pair);
                    }
                }
            });
        }
        catch (Exception e)
        {
            IsErrorVisible = true;
            ErrorMessage = $"Error loading family pairs: {e.Message}";
            _ = DelayVisibility();
        }
    }
}