namespace F1TelemetryApp.Model;

public class SectorTime : Time<ushort>
{
    public SectorTime(ushort time = 0)
    {
        Value = time;
    }

    public override bool UpdateTime(ushort time)
    {
        if (time == 0 || Value == time)
            return false;

        Value = time;
        return true;
    }
}
