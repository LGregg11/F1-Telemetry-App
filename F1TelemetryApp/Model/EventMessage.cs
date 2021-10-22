namespace F1_Telemetry_App.Model
{
    public struct EventMessage
    {
        public TelemetryReader.EventType EventType { get; set; }
        public int Total { get; set; }
    }
}
