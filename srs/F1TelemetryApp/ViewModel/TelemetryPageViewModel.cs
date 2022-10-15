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
    public int MyCarIndex = -1;

    public TelemetryPageViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        DriverCollection = new();
        Laps = new();
        DisplayedLap = new Lap(1);
        DisplayNewestLap = true;
        SessionTime = TelemetryConverter.ToTelemetryTime(0, true);
    }

    public event EventHandler? LapUpdated;
    public event EventHandler? DriverUpdated;

    public TelemetryDriverCollection DriverCollection { get; set; }
    public Dictionary<GraphDataType, GraphPointCollection> DataTypeSeriesCollectionMap => DriverCollection.GetGraphPointCollectionMap(DisplayedLapIndex);

    private string sessionTime;
    public string SessionTime
    {
        get => sessionTime;
        set
        {
            if (sessionTime != value)
            {
                sessionTime = value;
                RaisePropertyChanged();
            }
        }
    }

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
        tr.ParticipantPacket.Received -= OnParticipantReceived;

        tr.HeaderPacket.Received += OnHeaderReceived;
        tr.CarTelemetryPacket.Received += OnCarTelemetryReceived;
        tr.LapDataPacket.Received += OnLapDataReceived;
        tr.ParticipantPacket.Received += OnParticipantReceived;
    }

    private void OnHeaderReceived(object? sender, EventArgs e)
    {
        var header = ((HeaderEventArgs)e).Header;
        if (MyCarIndex < 0)
            MyCarIndex = header.playerCarIndex;
        App.Current.Dispatcher.Invoke(() =>
        {
            SessionTime = TelemetryConverter.ToTelemetryTime((int)header.sessionTime, true);
        });
    }

    private void OnParticipantReceived(object? sender, EventArgs e)
    {
        var participants = ((ParticipantEventArgs)e).Participant.participants;
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var participant in participants)
            {
                var name = participant.name;
                if (string.IsNullOrEmpty(participant.name) || DriverCollection.ContainsDriver(participant.name)) continue;

                DriverCollection.AddDriver(name, MyCarIndex);
                DriverUpdated?.Invoke(this, new EventArgs());
            }
        });
    }

    private void OnCarTelemetryReceived(object? sender, EventArgs e)
    {
        var carTelemetry = ((CarTelemetryEventArgs)e).CarTelemetry;
        App.Current.Dispatcher.Invoke(() =>
        {
            for (int i = 0; i <= GetDriverMaxIndex(carTelemetry.carTelemetryData.Length - 1); i++)
            {
                var carTelemetryData = carTelemetry.carTelemetryData[i];
                var driver = DriverCollection.GetDriver(i);
                if (driver == null) continue;
                driver.UpdateGraphPoint(GraphDataType.Throttle, null, carTelemetryData.throttle);
                driver.UpdateGraphPoint(GraphDataType.Brake, null, carTelemetryData.brake);
                driver.UpdateGraphPoint(GraphDataType.Gear, null, carTelemetryData.gear);
                driver.UpdateGraphPoint(GraphDataType.Speed, null, carTelemetryData.speed);
                driver.UpdateGraphPoint(GraphDataType.Steer, null, carTelemetryData.steer);
            }
        });
    }

    private void OnLapDataReceived(object? sender, EventArgs e)
    {
        var displayedLap = DisplayedLap;
        var lapData = ((LapDataEventArgs)e).LapData;
        App.Current.Dispatcher.Invoke(() =>
        {
            for (int i=0; i <= GetDriverMaxIndex(lapData.carLapData.Length - 1); i++)
            {
                var carLapData = lapData.carLapData[i];
                var lap = new Lap(carLapData.currentLapNum, carLapData.currentLapTime, carLapData.lapDistance);
                UpdateLap(lap);

                var driver = DriverCollection.GetDriver(i);
                if (driver == null) continue;

                if (lap == Laps.Last())
                    DriverCollection.AddLap(lap);

                driver.UpdateCurrentLapNumber(carLapData.currentLapNum);
                foreach (var type in DataTypeSeriesCollectionMap.Keys)
                    driver.UpdateGraphPoint(type, carLapData.lapDistance, null);
            }
        });
    }

    #region DEBUG

    public void DebugNewLap()
    {
        var lapNumber = 5;
        if (Laps.Count > 0)
            lapNumber = Laps.LastOrDefault()!.LapNumber + 1;
            
        UpdateLap(new Lap(lapNumber));
        DisplayedLap = Laps[DisplayedLapIndex];
    }

    public void DebugAddDrivers()
    {
        OnParticipantReceived(null, new ParticipantEventArgs
        {
            Participant = new F1GameTelemetry.Packets.F12021.Participant
            {
                participants = new F1GameTelemetry.Packets.F12021.ParticipantData[]
                {
                    new F1GameTelemetry.Packets.F12021.ParticipantData
                    {
                        name = "a"
                    },
                    new F1GameTelemetry.Packets.F12021.ParticipantData
                    {
                        name = "b"
                    },
                    new F1GameTelemetry.Packets.F12021.ParticipantData
                    {
                        name = "c"
                    },
                    new F1GameTelemetry.Packets.F12021.ParticipantData
                    {
                        name = "d"
                    }
                }
            }
        });
    }

    #endregion

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
        if (lap.LapNumber < 1) return;

        if (!Laps.Any(l => l.LapNumber == lap.LapNumber))
        {
            Laps.Add(lap);
            // UpdateFastestLap();

            if (DisplayNewestLap)
                DisplayedLapIndex = Laps.Count - 1;
            DisplayedLap = Laps[DisplayedLapIndex];
        }
        else
        {
            var existingLapIndex = Laps.IndexOf(Laps.FirstOrDefault(l => l.LapNumber == lap.LapNumber)!);
            Laps[existingLapIndex] = lap;
        }
    }

    //private void UpdateFastestLap()
    //{
    //    if (Laps.Count < 2) return;

    //    var lapsToCheck = Laps.SkipLast(1).ToList();
    //    var fastestLap = lapsToCheck.Select(l => l.LapTime).Min();
    //    for (int i = 0; i < lapsToCheck.Count; i++)
    //    {
    //        Laps[i].IsFastestLap = lapsToCheck[i].LapTime == fastestLap;
    //    }
    //}

    private int? TryGetIndexOfLap(int lapNum)
    {
        for (int i=0; i<Laps.Count; i++)
        {
            if (Laps[i].LapNumber == lapNum)
                return i;
        }

        return null;
    }

    private Lap? TryGetLap(int lapNum)
    {
        foreach (var lap in Laps)
        {
            if (lap.LapNumber == lapNum)
                return lap;
        }

        return null;
    }

    private int GetDriverMaxIndex(int val) => Math.Max(val, DriverCollection.Size - 1);
}
