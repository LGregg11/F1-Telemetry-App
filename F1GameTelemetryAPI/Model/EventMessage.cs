namespace F1TelF1GameTelemetryAPIemetryApp.Model;

using F1GameTelemetry.Enums;

public struct EventMessage
{
    public EventType EventType { get; set; }
    public int Total { get; set; }
}
