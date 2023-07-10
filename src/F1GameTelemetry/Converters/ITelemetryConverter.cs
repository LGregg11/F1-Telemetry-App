namespace F1GameTelemetry.Converters;

using Enums;
using Models;

public interface ITelemetryConverter
{
    string Name { get; }
    GameVersion GameVersion { get; }
    bool IsSupported { get; }
    bool IsExportEnabled { get; }
    int MaxCarsPerRace { get; }
    Header ConvertBytesToHeader(byte[] bytes);
    object? ConvertBytesToStandardPacket(PacketId packetType, byte[] bytes);
}
