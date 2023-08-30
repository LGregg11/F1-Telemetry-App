namespace F1TelemetryApp.Model;

public struct TelemetryMessage
{
    public ushort Speed { get; set; }
    public float Throttle { get; set; }
    public float Steer { get; set; }
    public float Brake { get; set; }
    public sbyte Gear { get; set; }
}
