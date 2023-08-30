namespace F1TelemetryApp.Model;

public class SectorTime : TimeData<ushort>
{
    public SectorTime(ushort time = 0)
    {
        Time = time;
    }

    public override bool UpdateTime(ushort time)
    {
        if (time == 0 || Time == time)
            return false;

        Time = time;
        return true;
    }
}
