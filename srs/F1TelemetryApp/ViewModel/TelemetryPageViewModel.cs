namespace F1TelemetryApp.ViewModel;

using Converters;
using Enums;
using Model;
using F1GameTelemetry.Listener;

using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using log4net;
using System;
using System.Collections.Generic;

public class TelemetryPageViewModel : BasePageViewModel
{
    private int myCarIndex = -1;
    private TelemetryMessage telemetryMessage = new() { Speed = 0, Brake = 0.0f, Throttle = 0.0f, Gear = 0, Steer = 0.0f };
    private LapDataMessage lapDataMessage = new() { CurrentLapTime = 0.0f, CurrentLapDistance = 0.0f };
    private double lapDistance = -1;

    public TelemetryPageViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        GraphPointCollections = new();
        foreach (DataGraphType type in Enum.GetValues(typeof(DataGraphType)))
            GraphPointCollections.Add(type, new GraphPointCollection(type));
    }

    public Dictionary<DataGraphType, GraphPointCollection> GraphPointCollections { get; set; } = new();
    public string LapTime => $"{TelemetryConverter.ToTelemetryTime(lapDataMessage.CurrentLapTime)}";
    public string LapDistance => $"{lapDataMessage.CurrentLapDistance:#0}";

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
            UpdateGraphPointY(DataGraphType.Throttle, carTelemetry.carTelemetryData[myCarIndex].throttle);
            UpdateGraphPointY(DataGraphType.Brake, carTelemetry.carTelemetryData[myCarIndex].brake);
            UpdateGraphPointY(DataGraphType.Gear, carTelemetry.carTelemetryData[myCarIndex].gear);
            UpdateGraphPointY(DataGraphType.Speed, carTelemetry.carTelemetryData[myCarIndex].speed);
            UpdateGraphPointY(DataGraphType.Steer, carTelemetry.carTelemetryData[myCarIndex].steer);
        });
    }

    private void OnLapDataReceived(object? sender, EventArgs e)
    {
        var lapData = ((LapDataEventArgs)e).LapData;
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var key in GraphPointCollections.Keys)
                UpdateGraphPointX(key, lapData.carLapData[myCarIndex].lapDistance);
            lapDataMessage.CurrentLapDistance = lapData.carLapData[myCarIndex].lapDistance;
            lapDataMessage.CurrentLapTime = lapData.carLapData[myCarIndex].currentLapTime;
        });
        RaisePropertyChanged(nameof(LapTime));
        RaisePropertyChanged(nameof(LapDistance));
    }

    private void UpdateGraphPointX(DataGraphType type, double x)
    {
        var point = GraphPointCollections[type].Point;
        point.X = x;
        GraphPointCollections[type].Point = point;
    }

    private void UpdateGraphPointY(DataGraphType type, double y)
    {
        var point = GraphPointCollections[type].Point;
        point.Y = y;
        GraphPointCollections[type].Point = point;
    }
}
