namespace F1GameTelemetry.Converters;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Packets.Standard;

public interface ITelemetryConverter
{
    string Name { get; }
    GameVersion GameVersion { get; }
    bool IsSupported { get; }
    bool IsExportEnabled { get; }
    int MaxCarsPerRace { get; }
    int HeaderPacketSize { get; }
    Header ConvertBytesToHeader(byte[] bytes);
    object? ConvertBytesToStandardPacket(PacketId packetType, byte[] bytes);
}
