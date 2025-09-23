using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Zoo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    public MainViewModel()
    {
        CurrentViewModel = App.ServiceProvider?.GetService<PetsViewModel>();
    }
}