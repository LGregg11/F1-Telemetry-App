namespace F1TelemetryApp.Model;

public class LapTime : TimeData<uint>
{
    public LapTime(uint time = 0)
    {
        Time = time;
    }

    public override bool UpdateTime(uint time)
    {
        if (time == 0 || Time == time)
            return false;

        Time = time;
        return true;
    }
}
