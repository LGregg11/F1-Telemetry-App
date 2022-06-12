namespace F1GameTelemetry.Readers;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Packets;

public interface ITelemetryReader
{
    string Name { get; }
    ITelemetryListener Listener { get; }
    bool IsSupported { get; }
    int MaxCarsPerRace { get; }
    int HeaderPacketSize { get; }

    // TODO: Replace this with an IEvent, and do an 'EventReceived' handler for all the events!
    EventType GetEventType(byte[] remainingPacket);
    void OnTelemetryReceived(object sender, TelemetryEventArgs e);
    IPacket HeaderPacket { get; }
    IPacket MotionPacket { get; }
    IPacket CarTelemetryPacket { get; }
    IPacket CarStatusPacket { get; }
    IPacket FinalClassificationPacket { get; }
    IPacket LapDataPacket { get; }
    IPacket SessionPacket { get; }
    IPacket ParticipantPacket { get; }
    IPacket SessionHistoryPacket { get; }
    IPacket LobbyInfoPacket { get; }
    IPacket CarDamagePacket { get; }
    IPacket CarSetupPacket { get; }
}
