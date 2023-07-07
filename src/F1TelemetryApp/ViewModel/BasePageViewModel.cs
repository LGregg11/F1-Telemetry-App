namespace F1TelemetryApp.ViewModel;

using log4net;
using Prism.Mvvm;
using System;

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
}
