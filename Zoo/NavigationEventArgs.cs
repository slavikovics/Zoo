using System;
using Avalonia.Controls;

namespace Zoo
{
    public class NavigationEventArgs : EventArgs
    {
        public object? PreviousViewModel { get; }
        public object? CurrentViewModel { get; }

        public NavigationEventArgs(object? previousViewModel, object? currentViewModel)
        {
            PreviousViewModel = previousViewModel;
            CurrentViewModel = currentViewModel;
        }
    }
}
