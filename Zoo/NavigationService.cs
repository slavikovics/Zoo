using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Zoo.ViewModels;

namespace Zoo;

public class NavigationService : INavigationService
{
    private ViewModelBase? _currentViewModel;
    
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TViewModel NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        var viewModel = _serviceProvider.GetService(typeof(TViewModel)) as TViewModel;

        _currentViewModel = viewModel ?? throw new InvalidOperationException(
            $"ViewModel {typeof(TViewModel).Name} is not registered in the service container.");
        return viewModel;
    }
}