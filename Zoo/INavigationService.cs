using System;
using Avalonia.Controls;
using Zoo.ViewModels;

namespace Zoo;

public interface INavigationService
{
    TViewModel NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
}