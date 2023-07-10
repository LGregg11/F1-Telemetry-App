namespace F1GameTelemetry.Models;


public struct FastestLap
{
    public FastestLap(byte vehicleIdx, float lapTime)
    {
        this.vehicleIdx = vehicleIdx;
        this.lapTime = lapTime;
    }

    public byte vehicleIdx;
    public float lapTime; // Seconds
}
