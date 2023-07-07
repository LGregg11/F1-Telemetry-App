namespace F1GameTelemetry.Packets.Standard;

using F1GameTelemetry.Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 946)]
public struct LapData
{
    public LapData(CarLapData[] carLapData)
    {
        this.carLapData = carLapData;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarLapData[] carLapData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 43)]
public struct CarLapData
{
    public CarLapData(
        uint lastLapTime,
        uint currentLapTime,
        ushort sector1Time,
        ushort sector2Time,
        float lapDistance,
        float totalDistance,
        float safetyCarDelta,
        byte carPosition,
        byte currentLapNum,
        PitStatus pitStatus,
        byte numPitStops,
        Sector sector,
        byte currentLapInvalid,
        byte penalties,
        byte warnings,
        byte numUnservedDriveThroughPenalties,
        byte numUnservedStopGoPenalties,
        byte gridPosition,
        DriverStatus driverStatus,
        ResultStatus resultStatus,
        byte pitLaneTimerActive,
        ushort pitLaneTimeInLane,
        ushort pitStopTimer,
        byte pitStopShouldServePen)
    {
        this.lastLapTime = lastLapTime;
        this.currentLapTime = currentLapTime;
        this.sector1Time = sector1Time;
        this.sector2Time = sector2Time;
        this.lapDistance = lapDistance;
        this.totalDistance = totalDistance;
        this.safetyCarDelta = safetyCarDelta;
        this.carPosition = carPosition;
        this.currentLapNum = currentLapNum;
        this.pitStatus = pitStatus;
        this.numPitStops = numPitStops;
        this.sector = sector;
        this.currentLapInvalid = currentLapInvalid;
        this.penalties = penalties;
        this.warnings = warnings;
        this.numUnservedDriveThroughPenalties = numUnservedDriveThroughPenalties;
        this.numUnservedStopGoPenalties = numUnservedStopGoPenalties;
        this.gridPosition = gridPosition;
        this.driverStatus = driverStatus;
        this.resultStatus = resultStatus;
        this.pitLaneTimerActive = pitLaneTimerActive;
        this.pitLaneTimeInLane = pitLaneTimeInLane;
        this.pitStopTimer = pitStopTimer;
        this.pitStopShouldServePen = pitStopShouldServePen;
    }

    // Times in milliseconds unless specified otherwise
    public uint lastLapTime;
    public uint currentLapTime;
    public ushort sector1Time;
    public ushort sector2Time;
    public float lapDistance; // distance around lap in metres
    public float totalDistance; // metres
    public float safetyCarDelta; // seconds
    public byte carPosition;
    public byte currentLapNum;
    public PitStatus pitStatus;
    public byte numPitStops;
    public Sector sector;
    public byte currentLapInvalid; // 0 = valid, 1 = invalid
    public byte penalties; // Accumulated time in seconds
    public byte warnings;
    public byte numUnservedDriveThroughPenalties;
    public byte numUnservedStopGoPenalties;
    public byte gridPosition;
    public DriverStatus driverStatus;
    public ResultStatus resultStatus;
    public byte pitLaneTimerActive; // 0 = inactive, 1 = active
    public ushort pitLaneTimeInLane;
    public ushort pitStopTimer;
    public byte pitStopShouldServePen;
}
