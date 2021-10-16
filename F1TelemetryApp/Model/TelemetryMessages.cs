namespace F1_Telemetry_App.Model
{
    public struct TelemetryMessages
    {
        public TelemetryReader.PacketIds PacketId { get; set; }
        public int Messages { get; set; }
    }
}
