namespace F1TelemetryApp.ViewModel;

using DataHandlers;
using Model;

using log4net;
using System.Linq;

public class TimesheetWindowViewModel : BasePageViewModel
{
    public TimesheetWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    }

    public static ObservableDriverCollection Drivers => DriversHandler.Drivers;

    public static ObservableString SessionTime => GeneralDataHandler.SessionTime; // TODO: Why is this not updating on the view - Do I need INotifyPropertyChanged?

    public override void SetTelemetryReader()
    {
        // Do nothing
    }
}
