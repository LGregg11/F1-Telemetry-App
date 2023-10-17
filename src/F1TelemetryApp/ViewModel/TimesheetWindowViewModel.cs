namespace F1TelemetryApp.ViewModel;

using Model;

using log4net;

public class TimesheetWindowViewModel : BasePageViewModel
{
    public TimesheetWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    }

    private ObservableDriverCollection _drivers = new();
    public ObservableDriverCollection Drivers
    {
        get => _drivers;
        set
        {
            if (_drivers != value)
            {
                _drivers = value;
                RaisePropertyChanged(nameof(Drivers));
            }
        }
    }

    public string SessionTime { get; set; }
}
