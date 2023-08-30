namespace F1TelemetryApp.Model;

using F1TelemetryApp.Enums;

using System;

public class TimeData<T>
{
    public TimeData()
    {
        Status = TimeStatus.Unknown;
    }

    public T Time { get; set; }
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
