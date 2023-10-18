namespace F1TelemetryApp.Model;

using Enums;

using F1GameTelemetry.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ObservableLapCollection : ObservableCollection<Lap>
{
    public ObservableLapCollection(int numSectors = 3)
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
    public Lap LastLapData { get; set; }
    public Lap CurrentLapData { get; set; }
    public Lap DisplayedLapData { get; set; }

    public void NewLap()
    {
        Add(new());

        if (Count > 0)
            CurrentLapData = this[Count - 1];

        if (Count > 1)
            LastLapData = this[Count - 2];
    }

    public void UpdateLapData(int index, LapHistoryData lapHistoryData)
    {
        if (index > Count - 1)
            throw new Exception($"Invalid index: {index}");

        this[index].UpdateLapNumber(Count);

        if (this[index].UpdateLapTime(lapHistoryData.lapTime))
            UpdateLapStatus(index);

        for (int s = 0; s < this[index].SectorTimes.Count; s++)
        {
            if (this[index].UpdateSectorTime(s, lapHistoryData.sectorTimes[s]))
                UpdateSectorStatus(index, s);
        }

        DisplayedLapData = CurrentLapData.SectorTimes[0].Value > 0 ? CurrentLapData : LastLapData;
    }

    public void UpdateLapStatus(int index)
    {
        var lapTime = this[index].LapTime;
        if (lapTime.Value == 0)
            return;

        if (BestLapTime.Value > 0 && lapTime.Value >= BestLapTime.Value)
        {
            this[index].LapTime.UpdateStatus(TimeStatus.NotPersonalBest);
            return;
        }

        this[BestLapIndex].UpdateLapStatus(TimeStatus.NotPersonalBest);
        this[index].UpdateLapStatus(TimeStatus.PersonalBest);
        BestLapIndex = index;
        BestLapTime = lapTime;
    }

    public void UpdateSectorStatus(int index, int s)
    {
        var sectorTime = this[index].SectorTimes[s];
        if (sectorTime.Value == 0)
            return;

        if (BestSectorTimes[s].Value > 0 && sectorTime.Value >= BestSectorTimes[s].Value)
        {
            this[index].UpdateSectorStatus(s, TimeStatus.NotPersonalBest);
            return;
        }

        this[BestSectorIndexes[s]].UpdateSectorStatus(s, TimeStatus.NotPersonalBest);
        this[index].UpdateSectorStatus(s, TimeStatus.PersonalBest);
        BestSectorIndexes[s] = index;
        BestSectorTimes[s] = sectorTime;
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
    }
}
