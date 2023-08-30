namespace F1TelemetryApp.Model;

using System;
using System.Collections.Generic;

public class TimesheetLapData
{
    // All times in milliseconds
    public TimesheetLapData(int numSectors = 3)
    {
        LapTime = new();
        SectorTimes = new(numSectors);
        for (int i = 0; i < numSectors; i++)
            SectorTimes.Add(new());
    }

    public LapTime LapTime { get; private set; }
    public List<SectorTime> SectorTimes { get; private set; }

    public bool UpdateLapTime(uint time)
    {
        return LapTime.UpdateTime(time);
    }

    public bool UpdateSectorTime(int index, ushort time)
    {
        if (index > SectorTimes.Count)
            throw new Exception($"{index} is out of range of SectorTimes (size {SectorTimes.Count})");

        return SectorTimes[index].UpdateTime(time);
    }
}
