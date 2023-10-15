namespace F1TelemetryApp.Model;

public class LapTime : Time<uint>
{
    public LapTime(uint time = 0)
    {
        Value = time;
    }

    public override bool UpdateTime(uint time)
    {
        if (time == 0 || Value == time)
            return false;

        Value = time;
        return true;
    }
}
