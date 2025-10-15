using System;
using Avalonia.Controls;
using Zoo.ViewModels;

namespace Zoo;

public interface INavigationService
{
    ViewModelBase NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
    
    ViewModelBase GoBack();
}