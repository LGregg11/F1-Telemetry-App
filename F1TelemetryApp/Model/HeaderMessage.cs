namespace F1TelemetryApp.Model
{
    using F1GameTelemetry.Reader;

    public struct HeaderMessage
    {
        public TelemetryReader.PacketIds PacketId { get; set; }
        public int Total { get; set; }
    }
}
