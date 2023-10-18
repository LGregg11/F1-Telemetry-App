namespace F1TelemetryApp.DataHandlers;

using F1GameTelemetry.Events;
using F1GameTelemetry.Models;
using F1GameTelemetry.Readers;

/// <summary>
/// A class that subscribes to the SingletonTelemetryReader class events and
/// updates the static data classes that can be used in multiple different views
/// </summary>
internal class AllDataHandler
{
    private ulong _sessionUID;

    public AllDataHandler()
    {
        SingletonTelemetryReader.ParticipantReceived += OnParticipantReceived;
        SingletonTelemetryReader.SessionHistoryReceived += OnSessionHistoryReceived;
        SingletonTelemetryReader.LapDataReceived += OnLapDataReceived;
    }

    ~AllDataHandler()
    {
        SingletonTelemetryReader.ParticipantReceived -= OnParticipantReceived;
        SingletonTelemetryReader.SessionHistoryReceived -= OnSessionHistoryReceived;
        SingletonTelemetryReader.LapDataReceived -= OnLapDataReceived;
    }


    private void OnParticipantReceived(object? sender, PacketEventArgs<Participant> e)
    {
        if (e.Header.sessionUID != _sessionUID)
        {
            _sessionUID = e.Header.sessionUID;

            DriversHandler.UpdateParticipant(e.Packet);
        }
    }

    private void OnSessionHistoryReceived(object? sender, PacketEventArgs<SessionHistory> e)
    {
        // Session History will give us the updated sector data
        // Session History packets are one per driver.
        DriversHandler.UpdateSessionHistory(e.Packet);
    }

    private void OnLapDataReceived(object sender, PacketEventArgs<LapData> e)
    {
        // Use lap data for header updates
        GeneralDataHandler.UpdateHeader(e.Header);

        // Only care about the position from this one for now
        DriversHandler.UpdateLapData(e.Packet);
    }
}
