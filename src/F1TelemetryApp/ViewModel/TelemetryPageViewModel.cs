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

    public TelemetryPageViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        Laps = new();
        DisplayedLap = new Lap(1);
        DisplayNewestLap = true;

        GraphPointCollectionMaps = new();
        UpdateGraphPointCollectionMaps();
    }

    public event EventHandler? LapUpdated;

    public List<Dictionary<DataGraphType, GraphPointCollection>> GraphPointCollectionMaps { get; set; }
    public Dictionary<DataGraphType, GraphPointCollection> GraphPointCollectionMap => GraphPointCollectionMaps[DisplayedLapIndex];

    public string LapTime => $"{TelemetryConverter.ToTelemetryTime(DisplayedLap.LapTime)}";
    public string LapDistance => $"{DisplayedLap.LapDistance}";

    private bool displayNewestLap;
    public bool DisplayNewestLap
    {
        get => displayNewestLap;
        set
        {
            displayNewestLap = value;
            RaisePropertyChanged();

            if (displayNewestLap)
                DisplayedLapIndex = Math.Max(0, Laps.Count - 1);
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

    private int displayedLapIndex;
    public int DisplayedLapIndex
    {
        get => displayedLapIndex;
        set
        {
            if (displayedLapIndex != value)
            {
                displayedLapIndex = value;
                RaisePropertyChanged();
                LapUpdated?.Invoke(this, new EventArgs());
            }
        }
    }

    private Lap displayedLap;
    public Lap DisplayedLap
    {
        get => displayedLap;
        set
        {
            if (value != null && displayedLap != value)
            {
                var lapNum = 1;
                if (displayedLap != null)
                    lapNum = displayedLap.LapNumber;

                displayedLap = value;
                RaisePropertyChanged();

                if (lapNum != value.LapNumber)
                {
                    DisplayedLapIndex = Laps.IndexOf(Laps.FirstOrDefault(l => l.LapNumber == value.LapNumber)!);
                    DisplayNewestLap = Laps.LastOrDefault()!.LapNumber == value.LapNumber;
                }

            }
        }
    }

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
            DisplayedLap = Laps[DisplayedLapIndex];

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
        DisplayedLap = Laps[DisplayedLapIndex];
    }

    public void RedrawLaps()
    {
        var laps = new ObservableCollection<Lap>();
        foreach (var lap in Laps)
            laps.Add(lap);

        Laps = laps;
        RaisePropertyChanged(nameof(DisplayedLap));
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
                DisplayedLapIndex = Laps.Count - 1;
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
        var fastestLap = lapsToCheck.Select(l => l.LapTime).Min();
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
