namespace F1TelemetryApp.Model.Timesheet;

using Enums;

using F1GameTelemetry.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;

public class LapDataCollection : List<TimesheetLapData>, INotifyPropertyChanged
{
    public LapDataCollection(int numSectors = 3)
    {
        ResetBestLap();
        ResetBestSectors(numSectors);

        LastLapData = new();
        CurrentLapData = new();
        DisplayedLapData = new();
    }

    public int BestLapIndex { get; private set; }
    public LapTime BestLapTime { get; private set; }
    public List<int> BestSectorIndexes { get; private set; }
    public List<SectorTime> BestSectorTimes { get; private set; }
    public int Laps => Count;
    public TimesheetLapData LastLapData { get; set; }
    public TimesheetLapData CurrentLapData { get; set; }
    public TimesheetLapData DisplayedLapData { get; set; }

    public void NewLap()
    {
        Add(new());

        if (Count > 0)
            CurrentLapData = this[Count - 1];

        if (Count > 1)
            LastLapData = this[Count - 2];

        NotifyPropertyChanged();
    }

    public void UpdateLapData(int index, LapHistoryData lapHistoryData)
    {
        if (index > Count - 1)
            throw new Exception($"Invalid index: {index}");

        if (this[index].UpdateLapTime(lapHistoryData.lapTime))
            UpdateLapStatus(index);

        for (int s = 0; s < this[index].SectorTimes.Count; s++)
        {
            if (this[index].UpdateSectorTime(s, lapHistoryData.sectorTimes[s]))
                UpdateSectorStatus(index, s);
        }

        DisplayedLapData = CurrentLapData.SectorTimes[0].Time > 0 ? CurrentLapData : LastLapData;

        NotifyPropertyChanged();
    }

    public void UpdateLapStatus(int index)
    {
        var lapTime = this[index].LapTime;
        if (lapTime.Time == 0)
            return;

        if (BestLapTime.Time > 0 && lapTime.Time >= BestLapTime.Time)
        {
            this[index].LapTime.UpdateStatus(TimeStatus.NotPersonalBest);
            return;
        }

        this[BestLapIndex].LapTime.UpdateStatus(TimeStatus.NotPersonalBest);
        this[index].LapTime.UpdateStatus(TimeStatus.PersonalBest);
        BestLapIndex = index;
        BestLapTime = lapTime;

        NotifyPropertyChanged();
    }

    public void UpdateSectorStatus(int index, int s)
    {
        var sectorTime = this[index].SectorTimes[s];
        if (sectorTime.Time == 0)
            return;

        if (BestSectorTimes[s].Time > 0 && sectorTime.Time >= BestSectorTimes[s].Time)
        {
            this[index].SectorTimes[s].UpdateStatus(TimeStatus.NotPersonalBest);
            return;
        }

        this[BestSectorIndexes[s]].SectorTimes[s].UpdateStatus(TimeStatus.NotPersonalBest);
        this[index].SectorTimes[s].UpdateStatus(TimeStatus.PersonalBest);
        BestSectorIndexes[s] = index;
        BestSectorTimes[s] = sectorTime;

        NotifyPropertyChanged();
    }

    private void ResetBestLap()
    {
        BestLapIndex = 0;
        BestLapTime = new();
    }

    private void ResetBestSectors(int numSectors)
    {
        BestSectorIndexes = new(numSectors);
        BestSectorTimes = new(numSectors);

        for (int i = 0; i < numSectors; i++)
        {
            BestSectorIndexes.Add(0);
            BestSectorTimes.Add(new());
        }

        NotifyPropertyChanged();
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
