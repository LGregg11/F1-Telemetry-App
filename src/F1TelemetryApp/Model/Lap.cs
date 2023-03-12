using System.ComponentModel;

namespace F1TelemetryApp.Model;

public class Lap
{
    public Lap(int lapNumber, float lapTime = 0.0f, float lapDistance = 0.0f)
    {
        LapNumber = lapNumber;
        LapTime = lapTime;
        LapDistance = lapDistance;
        IsFastestLap = false;
    }

    public int LapNumber { get; set; }
    public bool IsFastestLap { get; set; }
    public float LapTime { get; set; }
    public float LapDistance { get; set; }
}
