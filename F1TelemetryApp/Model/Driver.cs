namespace F1TelemetryApp.Model;

using System;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Packets.F12021;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

public class Driver : INotifyPropertyChanged
{
    public Driver(int index, ParticipantData p)
    {
        Index = index;
        Name = p.name;
        Nationality = p.nationality;
        Team = p.teamId;
        RaceNumber = p.raceNumber;
        ControlledBy = p.aiControlled;
        TelemetrySetting = p.yourTelemetry;
        DriverId = p.driverId;
    }
    public int Index { get; private set; }
    public string Name { get; private set; }
    public Nationality Nationality { get; private set; }
    public Team Team { get; private set; }
    public int RaceNumber { get; private set; }
    public AiControlled ControlledBy { get; private set; }
    public DriverId DriverId { get; private set; }
    public UdpSetting TelemetrySetting { get; set; } // If restricted, a lot of the data can be ignored.
    public int GridPosition { get; set; } = 0;
    public int Position { get; set; } = 0;
    public int Laps { get; set; } = 0;
    public Sector Sector { get; set; } = Sector.Sector1;
    public List<LapHistoryData> LapTimes { get; set; } = new List<LapHistoryData>();
    public int LastSector1Time { get; set; } = 0;
    public int LastSector2Time { get; set; } = 0;
    public int LastSector3Time { get; set; } = 0;
    public int LastLapTime { get; set; } = 0;
    public int CurrentLapTime { get; set; } = 0; // milliseconds
    public int BestSector1Lap { get; set; } = 0; // lap
    public int BestSector2Lap { get; set; } = 0; // lap
    public int BestSector3Lap { get; set; } = 0; // lap
    public int BestLapTimeLap { get; set; } = 0; // lap
    public int Warnings { get; set; } = 0;
    public int Penalties { get; set; } = 0;
    public DriverStatus DriverStatus { get; set; } = DriverStatus.Unknown;
    public ResultStatus ResultStatus { get; set; } = ResultStatus.Unknown;

    public event PropertyChangedEventHandler? PropertyChanged;

    public void ApplyCarLapData(CarLapData lapData)
    {
        if (GridPosition == 0)
            GridPosition = lapData.gridPosition;
        Position = lapData.carPosition;
        DriverStatus = lapData.driverStatus;
        ResultStatus = lapData.resultStatus;
        Sector = lapData.sector;
        CurrentLapTime = Convert.ToInt32(lapData.currentLapTime);
        NotifyPropertyChanged();
    }

    public void ApplySessionHistory(SessionHistory history)
    {
        if (history.lapHistoryData.Length < Index)
            throw new IndexOutOfRangeException($"{Index} of Driver {Name} is out of range for the most recent SessionHistory packet");

        if (Laps < history.numLaps)
        {
            // One final lap update before starting the new lap data
            if (Laps >= 1)
                LapTimes[Laps - 1] = history.lapHistoryData[Laps - 1];
            Laps = history.numLaps;
            LapTimes.Add(new LapHistoryData { lapTime = 0, sector1Time = 0, sector2Time = 0, sector3Time = 0 });
        }

        // Just completely update the lap data
        BestSector1Lap = history.bestSector1LapNum;
        BestSector2Lap = history.bestSector2LapNum;
        BestSector3Lap = history.bestSector3LapNum;
        BestLapTimeLap = history.bestLapTimeLapNum;
        LapTimes[Laps - 1] = history.lapHistoryData[Laps - 1];

        LastSector1Time = LapTimes.Select(l => l.sector1Time).Where(t => t > 0).LastOrDefault();
        LastSector2Time = LapTimes.Select(l => l.sector2Time).Where(t => t > 0).LastOrDefault();
        LastSector3Time = LapTimes.Select(l => l.sector3Time).Where(t => t > 0).LastOrDefault();
        LastLapTime = Convert.ToInt32(LapTimes.Select(l => l.lapTime).Where(t => t > 0).LastOrDefault());
        NotifyPropertyChanged();
    }

    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
