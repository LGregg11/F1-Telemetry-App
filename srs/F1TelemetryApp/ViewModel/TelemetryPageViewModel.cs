namespace F1TelemetryApp.ViewModel;

using Converters;
using Enums;
using Model;
using F1GameTelemetry.Listener;

using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

public class TelemetryPageViewModel : BasePageViewModel
{
    private int myCarIndex = -1;
    private bool isFirstLapData = true;

    public TelemetryPageViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        Laps = new();
        GraphPointCollectionMaps = new();
        UpdateGraphPointCollectionMaps();

        DisplayedLap = new Lap(1);
        DisplayNewestLap = true;
    }

    public event EventHandler? LapUpdated;

    public int DisplayedLapIndex => Laps.IndexOf(DisplayedLap) != -1 ? Laps.IndexOf(DisplayedLap) : 0;

    public List<Dictionary<DataGraphType, GraphPointCollection>> GraphPointCollectionMaps { get; set; }
    public Dictionary<DataGraphType, GraphPointCollection> GraphPointCollectionMap => GraphPointCollectionMaps[DisplayedLapIndex];

    private bool displayNewestLap;
    public bool DisplayNewestLap
    {
        get => displayNewestLap;
        set
        {
            displayNewestLap = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<Lap> laps;
    public ObservableCollection<Lap> Laps
    {
        get => laps;
        set
        {
            if (laps != value)
            {
                laps = value;
                RaisePropertyChanged();
            }
        }
    }

    private Lap displayedLap;
    public Lap DisplayedLap
    {
        get => displayedLap;
        set
        {
            displayedLap = value;
            LapUpdated?.Invoke(this, new EventArgs());
            RaisePropertyChanged();
        }
    }

    public string LapTime => $"{TelemetryConverter.ToTelemetryTime(DisplayedLap.LapTime)}";
    public string LapDistance => $"{DisplayedLap.LapDistance}";

    public override void SetTelemetryReader()
    {
        var tr = MainWindowViewModel.TelemetryReader;

        // TODO: This is wrong surely.. (Need to unsubscribe before the reader is changed)
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
        var myTelemetry = carTelemetry.carTelemetryData[myCarIndex];
        App.Current.Dispatcher.Invoke(() =>
        {
            UpdateGraphPointY(DataGraphType.Throttle, myTelemetry.throttle);
            UpdateGraphPointY(DataGraphType.Brake, myTelemetry.brake);
            UpdateGraphPointY(DataGraphType.Gear, myTelemetry.gear);
            UpdateGraphPointY(DataGraphType.Speed, myTelemetry.speed);
            UpdateGraphPointY(DataGraphType.Steer, myTelemetry.steer);
        });
    }

    private void OnLapDataReceived(object? sender, EventArgs e)
    {
        var displayedLap = DisplayedLap;
        var lapData = ((LapDataEventArgs)e).LapData;
        var myLapData = lapData.carLapData[myCarIndex];
        App.Current.Dispatcher.Invoke(() =>
        {
            var lap = new Lap(myLapData.currentLapNum, myLapData.currentLapTime, myLapData.lapDistance);
            UpdateLap(lap);
            DisplayedLap = Laps.FirstOrDefault(l => l.LapNumber == lap.LapNumber);

            foreach (var key in GraphPointCollectionMap.Keys)
                UpdateGraphPointX(key, myLapData.lapDistance);
        });

        RaisePropertyChanged(nameof(LapTime));
        RaisePropertyChanged(nameof(LapDistance));
    }

    public void DebugNewLap()
    {
        var lapNumber = 5;
        if (Laps.Count > 0)
            lapNumber = Laps.LastOrDefault()!.LapNumber + 1;
            
        UpdateLap(new Lap(lapNumber));
    }

    private void UpdateLap(Lap lap)
    {
        if (!Laps.Any(l => l.LapNumber == lap.LapNumber))
        {
            // New Lap - Add to Laps,
            Laps.Add(lap);
            UpdateFastestLap();

            if (Laps.Count > 1)
                UpdateGraphPointCollectionMaps();

            if (DisplayNewestLap)
                DisplayedLap = Laps.FirstOrDefault(l => l.LapNumber == Laps.LastOrDefault()!.LapNumber);
        }
        else
        {
            var existingLapIndex = Laps.IndexOf(Laps.FirstOrDefault(l => l.LapNumber == lap.LapNumber)!);
            Laps[existingLapIndex] = lap;
        }
    }

    private void UpdateFastestLap()
    {
        if (Laps.Count < 2) return;

        var lapsToCheck = Laps.SkipLast(1).ToList();
        var fastestLap = lapsToCheck.Select(l => l.LapTime).Max();
        for (int i = 0; i < lapsToCheck.Count; i++)
        {
            Laps[i].IsFastestLap = lapsToCheck[i].LapTime == fastestLap;
        }
    }

    private void UpdateGraphPointCollectionMaps()
    {
        GraphPointCollectionMaps.Add(CreateGraphPointCollectionMap());
        RaisePropertyChanged(nameof(GraphPointCollectionMaps));
    }

    private static Dictionary<DataGraphType, GraphPointCollection> CreateGraphPointCollectionMap()
    {
        var map = new Dictionary<DataGraphType, GraphPointCollection>();
        foreach (DataGraphType type in Enum.GetValues(typeof(DataGraphType)))
            map.Add(type, new GraphPointCollection(type));

        return map;
    }

    private void UpdateGraphPointX(DataGraphType type, double x)
    {
        var point = GraphPointCollectionMap[type].Point;
        point.X = x;
        GraphPointCollectionMap[type].Point = point;
    }

    private void UpdateGraphPointY(DataGraphType type, double y)
    {
        var point = GraphPointCollectionMap[type].Point;
        point.Y = y;
        GraphPointCollectionMap[type].Point = point;
    }
}
