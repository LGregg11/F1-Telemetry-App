namespace F1TelemetryApp.Model;

using Enums;

using F1GameTelemetry.Enums;

using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Lap : INotifyPropertyChanged
{
    // All times in milliseconds
    public Lap(int numSectors = 3)
    {
        LapNumber = 0;
        LapTime = new();
        SectorTimes = new();
        for (int i = 0; i < numSectors; i++)
            SectorTimes.Add(new());
        Tyre = TyreVisual.Soft;
    }

    public int LapNumber { get; set; }
    public LapTime LapTime { get; set; }
    public List<SectorTime> SectorTimes { get; set; }
    public TyreVisual Tyre { get; set; }

    public void UpdateLapNumber(int lapNumber)
    {
        if (LapNumber == 0)
        {
            LapNumber = lapNumber;
            NotifyAll();
        }
    }

    public bool UpdateLapTime(uint time)
    {
        bool result = LapTime.UpdateTime(time);

        NotifyAll();
        return result;
    }

    public bool UpdateLapStatus(TimeStatus status)
    {
        bool result = LapTime.UpdateStatus(status);

        NotifyAll();
        return result;
    }

    public bool UpdateSectorTime(int index, ushort time)
    {
        if (index > SectorTimes.Count)
            throw new Exception($"{index} is out of range of SectorTimes (size {SectorTimes.Count})");

        bool result = SectorTimes[index].UpdateTime(time);

        NotifyAll();
        return result;
    }

    public bool UpdateSectorStatus(int index, TimeStatus status)
    {
        bool result = SectorTimes[index].UpdateStatus(status);

        NotifyAll();
        return result;
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void NotifyAll()
    {
        NotifyPropertyChanged(nameof(LapNumber));
        NotifyPropertyChanged(nameof(LapTime));
        NotifyPropertyChanged(nameof(SectorTimes));
        NotifyPropertyChanged();
    }
    #endregion
}
