namespace F1GameTelemetry.Readers;

using System;
using System.Linq;
using System.Text;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Exporter;
using F1GameTelemetry.Packets;

public abstract class BaseTelemetryReader : ITelemetryReader
{
    public BaseTelemetryReader(ITelemetryListener listener, ITelemetryExporter exporter)
    {
        IsExportEnabled = false;
        Listener = listener;
        Exporter = exporter;
        Listener.TelemetryReceived += OnTelemetryReceived;
    }

    ~BaseTelemetryReader()
    {
        Listener.TelemetryReceived -= OnTelemetryReceived;
    }

    public ITelemetryListener Listener { get; }
    public ITelemetryExporter Exporter { get; }
    public abstract string Name { get; }
    public abstract bool IsSupported { get; }
    public abstract GameVersion GameVersion { get; }

    public bool IsExportEnabled { get; set; }
    public int MaxCarsPerRace => 22;
    public int HeaderPacketSize => 24;

    public abstract IPacket HeaderPacket { get; }
    public abstract IPacket MotionPacket { get; }
    public abstract IPacket CarTelemetryPacket { get; }
    public abstract IPacket CarStatusPacket { get; }
    public abstract IPacket FinalClassificationPacket { get; }
    public abstract IPacket LapDataPacket { get; }
    public abstract IPacket SessionPacket { get; }
    public abstract IPacket ParticipantPacket { get; }
    public abstract IPacket SessionHistoryPacket { get; }
    public abstract IPacket LobbyInfoPacket { get; }
    public abstract IPacket CarDamagePacket { get; }
    public abstract IPacket CarSetupPacket { get; }

    public abstract void OnTelemetryReceived(object sender, TelemetryEventArgs e);
    public abstract void RaiseEventHandler(PacketId id, byte[] remainingPacket);

    public EventType GetEventType(byte[] remainingPacket)
    {
        try
        {
            return (EventType)Enum.Parse(
                typeof(EventType),
                Encoding.ASCII.GetString(remainingPacket.Take(4).ToArray())
                );
        }
        catch
        {
            // Return an unknown event type instead of an error
            return EventType.UNKNOWN;
        }
    }
}
