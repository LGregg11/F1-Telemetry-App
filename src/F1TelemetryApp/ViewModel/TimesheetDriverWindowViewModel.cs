namespace F1TelemetryApp.ViewModel;

using log4net;

public class TimesheetDriverWindowViewModel : BasePageViewModel
{
    public TimesheetDriverWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        DriverIndex = -1;
        TimesheetWindowViewModel = new();
    }

    private TimesheetWindowViewModel _timesheetWindowViewModel;
    public TimesheetWindowViewModel TimesheetWindowViewModel
    {
        get => _timesheetWindowViewModel;
        set
        {
            if (_timesheetWindowViewModel != value)
            {
                _timesheetWindowViewModel = value;
                RaisePropertyChanged(nameof(TimesheetWindowViewModel));
            }
        }
    }
    public int DriverIndex { get; internal set; }

    public string Position => TimesheetWindowViewModel.GetDriverPosition(DriverIndex).ToString();
    public string Laps => TimesheetWindowViewModel.GetDriverLap(DriverIndex).ToString();
}
