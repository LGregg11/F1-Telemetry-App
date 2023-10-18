namespace F1TelemetryApp.ViewModel;

using DataHandlers;
using Model;

using log4net;
using System.ComponentModel;

public class TimesheetDriverWindowViewModel : BasePageViewModel
{
    public TimesheetDriverWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    }

    private byte _driverIndex;
    private Driver Driver => DriversHandler.Drivers[_driverIndex];
    public ObservableString Position => new(Driver.Position.ToString());
    public ObservableString Laps => new(Driver.LapData.Laps.ToString());
    public ObservableLapCollection LapData => Driver.LapData;


    public void SubscribeToDriver(byte driverIndex)
    {
        _driverIndex = driverIndex;
        RaisePropertyChanged(nameof(Position));
        RaisePropertyChanged(nameof(Laps));
        RaisePropertyChanged(nameof(LapData));
    }
}
