namespace F1TelemetryApp.Model;

using Enums;

using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ObservableDriverCollection : ObservableCollection<Driver>
{
    private int _sessionBestLapDriverIndex;
    private LapTime _sessionBestLapTime;

    private List<int> _sessionBestSectorIndexes;
    private List<SectorTime> _sessionBestSectorTimes;

    public ObservableDriverCollection()
    {
        ResetBestLap();
        ResetBestSectors();
    }

    public void UpdateFastestLap(int i)
    {
        // Check current fastest lap with this driver's fastest lap
        if (Count == 0)
            return;

        var driverBestLapTime = this[i].LapData.BestLap.LapTime;
        if (_sessionBestLapTime.Value > 0 && driverBestLapTime.Value >= _sessionBestLapTime.Value)
        {
            this[_sessionBestLapDriverIndex].LapData.DisplayedLapData.NotifyAll();
            this[i].LapData.DisplayedLapData.NotifyAll();
            return;
        }

        this[_sessionBestLapDriverIndex].LapData.BestLap.UpdateLapStatus(TimeStatus.PersonalBest);
        this[i].LapData.DisplayedLapData.UpdateLapStatus(TimeStatus.BestOfSession);
        _sessionBestLapTime = driverBestLapTime;
        _sessionBestLapDriverIndex = i;
    }

    public void UpdateFastestSector(int sector, int i)
    {
        if (Count == 0)
            return;

        var driverBestSectorTime = this[i].LapData.BestSectorLap[sector].SectorTimes[sector];
        if (_sessionBestSectorTimes[sector].Value > 0 && driverBestSectorTime.Value >= _sessionBestSectorTimes[sector].Value)
        {
            this[_sessionBestSectorIndexes[sector]].LapData.DisplayedLapData.NotifyAll();
            this[i].LapData.DisplayedLapData.NotifyAll();
            return;
        }

        this[_sessionBestSectorIndexes[sector]].LapData.BestSectorLap[sector].UpdateSectorStatus(sector, TimeStatus.PersonalBest);
        this[i].LapData.DisplayedLapData.UpdateSectorStatus(sector, TimeStatus.BestOfSession);

        _sessionBestSectorTimes[sector] = driverBestSectorTime;
        _sessionBestSectorIndexes[sector] = i;
    }

    public void Reset()
    {
        Clear();
        ResetBestLap();
        ResetBestSectors();
    }

    private void ResetBestLap()
    {
        _sessionBestLapDriverIndex = 0;
        _sessionBestLapTime = new();
    }

    private void ResetBestSectors()
    {
        var sectors = 3;

        _sessionBestSectorIndexes = new(sectors);
        _sessionBestSectorTimes = new(sectors);

        for (int i = 0; i < sectors; i++)
        {
            _sessionBestSectorIndexes.Add(0);
            _sessionBestSectorTimes.Add(new());
        }
    }
}
