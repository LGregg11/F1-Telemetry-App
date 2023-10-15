namespace F1TelemetryApp.ViewModel;

using Model;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Events;
using F1GameTelemetry.Models;
using F1GameTelemetry.Readers;

using log4net;

public class TimesheetWindowViewModel : BasePageViewModel
{
    private const int _numSectors = 3;
    private ulong _sessionUID;

    public TimesheetWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    }

    private ObservableDriverCollection _drivers = new();
    public ObservableDriverCollection Drivers
    {
        get => _drivers;
        set
        {
            if (_drivers != value)
            {
                _drivers = value;
                RaisePropertyChanged();
            }
        }
    }

    public string SessionTime { get; set; }

    public override void SetTelemetryReader()
    {
        SingletonTelemetryReader.ParticipantReceived += OnParticipantReceived;
        SingletonTelemetryReader.SessionHistoryReceived += OnSessionHistoryReceived;
        SingletonTelemetryReader.LapDataReceived += OnLapDataReceived;
    }

    internal int GetDriverPosition(int driverIndex)
    {
        if (driverIndex < 0 || driverIndex >= Drivers.Count)
            return 0;

        return Drivers[driverIndex].Position;
    }

    internal int GetDriverLap(int driverIndex)
    {
        if (driverIndex < 0 || driverIndex >= Drivers.Count)
            return 0;

        return Drivers[driverIndex].LapData.Laps;
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

                Drivers.Add(new Driver(participantName.Replace("\0", string.Empty), i));
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
            var driver = Drivers[driverIndex];
            var lapOfFastestSectors = new int[_numSectors];
            for (byte i = 0; i < _numSectors; i++)
                lapOfFastestSectors[i] = driver.LapData.BestSectorIndexes[i];

            Drivers[driverIndex].UpdateLapHistoryData(
                sessionHistoryPacket.numLaps,
                sessionHistoryPacket.lapHistoryData);

            if (driver.LapData.BestLapIndex == sessionHistoryPacket.bestLapTimeLapNum - 1)
                Drivers.UpdateFastestLap(driverIndex);

            for (byte i=0; i < _numSectors; i++)
            {
                if (lapOfFastestSectors[i] == sessionHistoryPacket.bestSectorTimeLapNums[i] - 1)
                    Drivers.UpdateFastestSector(i, driverIndex);
            }

            Drivers[driverIndex].UpdateTyreStintHistoryData(
                sessionHistoryPacket.numTyreStints,
                sessionHistoryPacket.tyreStintHistoryData);
        });

        RaisePropertyChanged(nameof(Drivers));
    }

    private void OnLapDataReceived(object sender, PacketEventArgs<F1GameTelemetry.Models.LapData> e)
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
