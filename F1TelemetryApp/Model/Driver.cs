namespace F1TelemetryApp.Model;

using System;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Packets.F12021;
using System.Collections.Generic;
using System.Linq;

public class Driver
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
    public List<LapTime> LapTimes { get; set; } = new List<LapTime>();
    public int BestSector1 { get; set; } = 0;
    public int BestSector2 { get; set; } = 0;
    public int BestSector3 { get; set; } = 0;
    public int BestFullLap { get; set; } = 0;
    public int Warnings { get; set; } = 0;
    public int Penalties { get; set; } = 0;
    public DriverStatus DriverStatus { get; set; } = DriverStatus.Unknown;
    public ResultStatus ResultStatus { get; set; } = ResultStatus.Unknown;

    public void ApplyLapData(LapData lapData)
    {
        // If race has finished, we want to make sure no more lap data is processed (just in case!)
        if (ResultStatus == ResultStatus.Finished) return;

        if (lapData.carLapData.Length < Index)
            throw new IndexOutOfRangeException($"{Index} of Driver {Name} is out of range for the most recent LapData packet");

        var driverData = lapData.carLapData[Index];

        // Position
        if (GridPosition == 0)
            GridPosition = driverData.gridPosition;
        Position = driverData.carPosition;

        // Status
        DriverStatus = driverData.driverStatus;
        ResultStatus = driverData.resultStatus;

        if (Laps < driverData.currentLapNum)
        {
            // Started a new lap? Create a new LapTime and update Laps
            Laps = driverData.currentLapNum;
            LapTimes.Add(new LapTime { Sector1 = 0f, Sector2 = 0f, Sector3 = 0f, TotalLapTime = 0f });

            if (Laps < 2) return;
            var previousLap = LapTimes[Laps - 2];
            previousLap.TotalLapTime = driverData.lastLapTime;
            previousLap.Sector3 = previousLap.TotalLapTime - (previousLap.Sector1 - previousLap.Sector2);
            LapTimes[Laps - 2] = previousLap;
            BestSector3 = UpdateBestLap(LapTimes.Select(l => l.Sector3).ToArray());
            BestFullLap = UpdateBestLap(LapTimes.Select(l => l.TotalLapTime).ToArray());
        }

        var currentLap = LapTimes[Laps - 1];

        // Only process sector 1 and 2 lap times if the sector has changed
        if (Sector != driverData.sector)
        {
            Sector = driverData.sector;
            if (Sector == Sector.Sector2)
            {
                currentLap.Sector1 = driverData.sector1Time;
                LapTimes[Laps - 1] = currentLap;
                BestSector1 = UpdateBestLap(LapTimes.Select(l => l.Sector1).ToArray());
            }
            else if (Sector == Sector.Sector3)
            {
                currentLap.Sector2 = driverData.sector2Time;
                LapTimes[Laps - 1] = currentLap;
                BestSector2 = UpdateBestLap(LapTimes.Select(l => l.Sector2).ToArray());
            }
        }

        // If the driver has finished the race, we want to process their last sector 3 and lap time
        if (ResultStatus == ResultStatus.Finished)
        {
            currentLap.TotalLapTime = driverData.currentLapTime;
            currentLap.Sector3 = currentLap.TotalLapTime - (currentLap.Sector1 - currentLap.Sector2);
            LapTimes[Laps - 1] = currentLap;
            BestSector3 = UpdateBestLap(LapTimes.Select(l => l.Sector3).ToArray());
            BestFullLap = UpdateBestLap(LapTimes.Select(l => l.TotalLapTime).ToArray());
        }
    }

    private static int UpdateBestLap(float[] values)
    {
        var minIndex = 0;
        if (values == null || values.Length < 2) return minIndex + 1;

        var min = values[0];
        for (var i = 1; i < values.Length; i++)
        {
            if (values[i] < min)
            {
                min = values[i];
                minIndex = i;
            }
        }

        return minIndex + 1;
    }
}
