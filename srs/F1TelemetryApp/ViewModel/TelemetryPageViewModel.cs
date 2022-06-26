namespace F1TelemetryApp.ViewModel;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Listener;
using F1TelemetryApp.Converters;
using F1TelemetryApp.Model;
using log4net;
using System;

public class TelemetryPageViewModel : BasePageViewModel
{
    private int myCarIndex = -1;
    private TelemetryMessage telemetryMessage = new() { Speed = 0, Brake = 0.0f, Throttle = 0.0f, Gear = 0, Steer = 0.0f };
    private LapDataMessage lapDataMessage = new() { CurrentLapTime = 0.0f, CurrentLapDistance = 0.0f };

    public TelemetryPageViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    }

    public string LapTime => $"{TelemetryConverter.ToTelemetryTime(lapDataMessage.CurrentLapTime)}";
    public string LapDistance => $"{lapDataMessage.CurrentLapDistance:#0}";
    public string Speed => $"{telemetryMessage.Speed}";
    public string Throttle => $"{telemetryMessage.Throttle}";
    public string Brake => $"{telemetryMessage.Brake}";
    public string Gear => $"{telemetryMessage.Gear}";
    public string Steer => $"{telemetryMessage.Steer}";

    public override void SetTelemetryReader()
    {
        var tr = MainWindowViewModel.TelemetryReader;

        tr.HeaderPacket.Received -= OnHeaderReceived;
        tr.CarTelemetryPacket.Received -= OnCarTelemetryReceived;
        tr.LapDataPacket.Received -= OnLapDataReceived;

        tr.HeaderPacket.Received += OnHeaderReceived;
        tr.CarTelemetryPacket.Received += OnCarTelemetryReceived;
        tr.LapDataPacket.Received += OnLapDataReceived;
    }

    private void OnHeaderReceived(object? sender, EventArgs e)
    {
        var header = ((HeaderEventArgs)e).Header;
        if (myCarIndex < 0)
            myCarIndex = header.playerCarIndex;
    }

    private void OnCarTelemetryReceived(object? sender, EventArgs e)
    {
        var carTelemetry = ((CarTelemetryEventArgs)e).CarTelemetry;
        App.Current.Dispatcher.Invoke(() =>
        {
            telemetryMessage.Speed = carTelemetry.carTelemetryData[myCarIndex].speed;
            telemetryMessage.Throttle = carTelemetry.carTelemetryData[myCarIndex].throttle;
            telemetryMessage.Brake = carTelemetry.carTelemetryData[myCarIndex].brake;
            telemetryMessage.Gear = carTelemetry.carTelemetryData[myCarIndex].gear;
            telemetryMessage.Steer = carTelemetry.carTelemetryData[myCarIndex].steer;
        });
        RaisePropertyChanged(nameof(Speed));
        RaisePropertyChanged(nameof(Throttle));
        RaisePropertyChanged(nameof(Brake));
        RaisePropertyChanged(nameof(Gear));
        RaisePropertyChanged(nameof(Steer));
    }
    private void OnLapDataReceived(object? sender, EventArgs e)
    {
        var lapData = ((LapDataEventArgs)e).LapData;
        App.Current.Dispatcher.Invoke(() =>
        {
            lapDataMessage.CurrentLapDistance = lapData.carLapData[myCarIndex].lapDistance;
            lapDataMessage.CurrentLapTime = lapData.carLapData[myCarIndex].currentLapTime;
        });
        RaisePropertyChanged(nameof(LapTime));
        RaisePropertyChanged(nameof(LapDistance));
    }
}
