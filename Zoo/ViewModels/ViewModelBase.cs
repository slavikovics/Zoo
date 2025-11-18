using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Zoo.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private string _errorMessage;

    [ObservableProperty] private bool _isErrorVisible;

    private CancellationTokenSource? _errorTimerCts;

    public async Task DelayVisibility()
    {
        _errorTimerCts?.Cancel();
        _errorTimerCts = new CancellationTokenSource();
        var token = _errorTimerCts.Token;

        try
        {
            await Task.Delay(1000, token);
            IsErrorVisible = false;
        }
        catch (TaskCanceledException)
        {
        }
    }
}