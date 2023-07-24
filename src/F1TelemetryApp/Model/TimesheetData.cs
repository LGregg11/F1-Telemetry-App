namespace F1TelemetryApp.Model;

public struct TimesheetData
{
    public int Laps { get; set; }
    public ushort Sector1Time { get; set; }
    public ushort Sector2Time { get; set; }
    public ushort Sector3Time { get; set; }
    public uint LastLapTime { get; set; }
    public uint BestLapTime { get; set; }
}
