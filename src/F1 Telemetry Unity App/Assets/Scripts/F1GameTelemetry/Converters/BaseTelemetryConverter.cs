namespace F1GameTelemetry.Converters
{
    using Enums;
    using Models;

    using System;
    using System.Linq;
    using System.Text;

#nullable enable

    public abstract class BaseTelemetryConverter : ITelemetryConverter
    {
        public BaseTelemetryConverter()
        {
            IsExportEnabled = false;
        }

        public abstract string Name { get; }
        public abstract GameVersion GameVersion { get; }
        public abstract int PacketHeaderSize { get; }
        public bool IsExportEnabled { get; set; }
        public int MaxCarsPerRace => 22;

        public abstract Header ConvertBytesToHeader(byte[] bytes);

        public abstract object? ConvertBytesToStandardPacket(PacketId packetType, byte[] bytes);

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
}
