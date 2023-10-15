namespace F1TelemetryApp.Model;

using F1TelemetryApp.Enums;

using System;

public class Time<T>
{
    public Time()
    {
        Status = TimeStatus.Unknown;
    }

    public T Value { get; set; }
    public TimeStatus Status { get; set; }

    public virtual bool UpdateTime(T time) => throw new NotImplementedException();

    public bool UpdateStatus(TimeStatus status)
    {
        if (Status == status)
            return false;

        Status = status;
        return true;
    }
}
