using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Zoo.ViewModels;

namespace Zoo;

public class NavigationService : INavigationService
{
    private ViewModelBase? _previousViewModel;
    
    private ViewModelBase? _currentViewModel;
    
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ViewModelBase NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        var viewModel = _serviceProvider.GetService(typeof(TViewModel)) as ViewModelBase;

        _previousViewModel = _currentViewModel;

        _currentViewModel = viewModel ?? throw new InvalidOperationException(
            $"ViewModel {typeof(TViewModel).Name} is not registered in the service container.");
        return viewModel;
    }

    public ViewModelBase GoBack()
    {
        _currentViewModel = _previousViewModel;
        _previousViewModel = null;

        if (_currentViewModel is null) throw new InvalidOperationException("Cannot go back to a null view model.");
        return _currentViewModel;
    }
}