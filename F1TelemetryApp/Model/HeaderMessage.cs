namespace F1_Telemetry_App.Model
{
    public struct HeaderMessage
    {
        public TelemetryReader.PacketIds PacketId { get; set; }
        public int Total { get; set; }
    }
}
