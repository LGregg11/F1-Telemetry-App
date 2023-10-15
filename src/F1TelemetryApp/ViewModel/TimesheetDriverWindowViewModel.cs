namespace F1TelemetryApp.ViewModel;

using log4net;

public class TimesheetDriverWindowViewModel : BasePageViewModel
{
    public TimesheetDriverWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    }
}
