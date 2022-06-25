using log4net;

namespace F1TelemetryApp.ViewModel;

public class TelemetryPageViewModel : BasePageViewModel
{
    public TelemetryPageViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    }

    public override void SetTelemetryReader()
    {
        // Lap Distance
        // Lap Time
        // Throttle
        // Brake
        // Gear
        // Steering
    }
}
