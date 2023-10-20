namespace F1GameTelemetry.Converters
{
    using Enums;
    using Models;

#nullable enable

    public interface ITelemetryConverter
    {
        string Name { get; }
        GameVersion GameVersion { get; }
        bool IsExportEnabled { get; }
        int PacketHeaderSize { get; }
        int MaxCarsPerRace { get; }
        Header ConvertBytesToHeader(byte[] bytes);
        object? ConvertBytesToStandardPacket(PacketId packetType, byte[] bytes);
    }
}
