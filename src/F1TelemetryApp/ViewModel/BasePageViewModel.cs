namespace F1TelemetryApp.ViewModel;

using log4net;
using Prism.Mvvm;

using System;
using System.Threading.Tasks;

public class BasePageViewModel : BindableBase
{
    public ILog Log { get; set; }

    private MainWindowViewModel _mainWindowViewModel;
    public MainWindowViewModel MainWindowViewModel
    {
        get => _mainWindowViewModel;

        set
        {
            _mainWindowViewModel = value;
            SetTelemetryReader();
        }
    }

    public virtual void SetTelemetryReader() => throw new NotImplementedException();

    protected static void InvokeDispatcher(Action action)
    {
        App.Current.Dispatcher.Invoke(action);
    }

    protected static void InvokeDispatcherAsync(Action action)
    {
        App.Current.Dispatcher.Invoke(async () =>
        {
            await Task.Run(action);
        });
    }
}
