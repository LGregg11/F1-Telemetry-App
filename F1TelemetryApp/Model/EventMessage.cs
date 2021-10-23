namespace F1TelemetryApp.Model
{
    using F1GameTelemetry.Reader;

    public struct EventMessage
    {
        public TelemetryReader.EventType EventType { get; set; }
        public int Total { get; set; }
    }
}
