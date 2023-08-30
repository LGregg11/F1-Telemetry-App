namespace F1TelemetryApp.Model;

using System.Collections.Generic;

public struct TimesheetData
{
    public int Laps { get; set; }
    public List<ushort> SectorTimes { get; set; }
    public uint LastLapTime { get; set; }
    public uint BestLapTime { get; set; }
}
