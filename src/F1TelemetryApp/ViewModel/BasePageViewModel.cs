namespace F1TelemetryApp.ViewModel;

using F1TelemetryApp.Interfaces;
using log4net;
using Prism.Mvvm;
using System;

public class BasePageViewModel : BindableBase, IPageViewModel
{
    public ILog Log { get; set; }

    private MainWindowViewModel mainWindowViewModel;
    public MainWindowViewModel MainWindowViewModel
    {
        get => mainWindowViewModel;

        set
        {
            mainWindowViewModel = value;
            SetTelemetryReader();
        }
    }

    public virtual void SetTelemetryReader() => throw new NotImplementedException();
}
