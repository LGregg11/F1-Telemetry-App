namespace F1TelemetryApp.Model;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Models;

using System.Collections.Generic;
using System.ComponentModel;

public class TimesheetDriver : INotifyPropertyChanged
{
    private int _bestLapTimeLapNumber = 1;

    public TimesheetDriver(string name, byte arrayIndex)
    {
        Name = name;
        ArrayIndex = arrayIndex;

        LapData = new();
    }

    public string Name { get; init; }
    public byte ArrayIndex { get; init; }
    public int Position { get; set; }
    public List<LapHistoryData> LapData { get; set; }
    public TimesheetData TimesheetData { get; set; }
    public TyreVisual CurrentTyre { get; set; }

    public void SetData(LapHistoryData[] lapData, byte numLaps, TyreStintHistoryData[] tyreData, byte numTyreStints)
    {
        var lapDataList = new List<LapHistoryData>();

        for (byte i = 0; i < numLaps; i++)
            lapDataList.Add(lapData[i]);

        uint lastLapTime = 0;
        if (numLaps > 1)
            lastLapTime = lapDataList[numLaps - 2].lapTime;

        var lap = numLaps - 1;
        if (lap > 0 && lapDataList[lap].sector1Time == 0)
            lap--;

        LapData = lapDataList;
        TimesheetData = new TimesheetData()
        {
            Laps = numLaps,
            Sector1Time = lapDataList[lap].sector1Time,
            Sector2Time = lapDataList[lap].sector2Time,
            Sector3Time = lapDataList[lap].sector3Time,
            LastLapTime = lastLapTime,
            BestLapTime = lapDataList[_bestLapTimeLapNumber - 1].lapTime
        };

        var currentTyre = tyreData[numTyreStints - 1].tyreVisualCompound;
        if (CurrentTyre != currentTyre)
            CurrentTyre = currentTyre;
        NotifyPropertyChanged();
    }

    public void SetBestLapTimeLapNum(int lap)
    {
        if (lap == 0 || _bestLapTimeLapNumber == lap)
            return;

        _bestLapTimeLapNumber = lap;

        if (LapData.Count == 0)
            return;

        TimesheetData = new TimesheetData()
        {
            Laps = TimesheetData.Laps,
            Sector1Time = TimesheetData.Sector1Time,
            Sector2Time = TimesheetData.Sector2Time,
            Sector3Time = TimesheetData.Sector3Time,
            LastLapTime = TimesheetData.LastLapTime,
            BestLapTime = LapData[_bestLapTimeLapNumber - 1].lapTime
        };
        NotifyPropertyChanged();
    }

    public void SetPosition(int carPosition)
    {
        if (Position == carPosition)
            return;

        Position = carPosition;
        NotifyPropertyChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
