namespace F1TelemetryApp.ViewModel;

using Model;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Events;
using F1GameTelemetry.Models;
using F1GameTelemetry.Readers;

using log4net;

using System.Collections.ObjectModel;
using System;
using System.Diagnostics;

public class TimesheetWindowViewModel : BasePageViewModel
{
    private ulong _sessionUID;

    public TimesheetWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    }

    // private public drivers observable collection
    private ObservableCollection<TimesheetDriver> _drivers = new();
    public ObservableCollection<TimesheetDriver> Drivers
    {
        get => _drivers;
        set
        {
            if (_drivers != value)
                _drivers = value;
        }
    }

    public string SessionTime { get; set; }

    public override void SetTelemetryReader()
    {
        SingletonTelemetryReader.ParticipantReceived += OnParticipantReceived;
        SingletonTelemetryReader.SessionHistoryReceived += OnSessionHistoryReceived;
        SingletonTelemetryReader.LapDataReceived += OnLapDataReceived;
    }

    private void OnParticipantReceived(object? sender, PacketEventArgs<Participant> e)
    {
        // If session UID is the same, the paricipants will not have changed
        if (e.Header.sessionUID == _sessionUID)
            return;

        _sessionUID = e.Header.sessionUID;

        // Participant packets will give us the names of the drivers
        var participantPacket = e.Packet;

        InvokeDispatcher(() =>
        {
            Drivers.Clear();

            for (byte i = 0; i < participantPacket.numActiveCars; i++)
            {
                var participantName = participantPacket.participants[i].name;
                if (string.IsNullOrEmpty(participantName))
                    return;

                Drivers.Add(new TimesheetDriver(participantName, i));
            }
        });

        RaisePropertyChanged(nameof(Drivers));
    }

    private void OnSessionHistoryReceived(object? sender, PacketEventArgs<SessionHistory> e)
    {
        // Session History will give us the updated sector data
        // Session History packets are one per driver.

        var sessionHistoryPacket = e.Packet;

        var driverIndex = sessionHistoryPacket.carIdx;
        if (!CheckDriverExists(driverIndex)) return;

        InvokeDispatcherAsync(() =>
        {
            Drivers[driverIndex].SetData(
                sessionHistoryPacket.lapHistoryData,
                sessionHistoryPacket.numLaps,
                sessionHistoryPacket.tyreStintHistoryData,
                sessionHistoryPacket.numTyreStints
                );
            Drivers[driverIndex].SetBestLapTimeLapNum(sessionHistoryPacket.bestLapTimeLapNum);
        });

        RaisePropertyChanged(nameof(Drivers));
    }

    private void OnLapDataReceived(object sender, PacketEventArgs<LapData> e)
    {
        // Only care about the position from this one for now

        var lapDataPacket = e.Packet;

        InvokeDispatcherAsync(() =>
        {
            SessionTime = (e.Header.sessionTime * 1000).ToTelemetryTime();

            for (int i = 0; i < Drivers.Count; i++)
                Drivers[i].SetPosition(lapDataPacket.carLapData[i].carPosition);
        });

        RaisePropertyChanged(nameof(SessionTime));
        RaisePropertyChanged(nameof(Drivers));
    }

    private bool CheckDriverExists(byte index) => Drivers.Count > index;
}
