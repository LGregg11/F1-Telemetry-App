namespace F1GameTelemetry.Packets.F12023;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1100)]
public struct LapData
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarLapData[] carLapData;

    public byte timeTrialPBCarIdx; // Index of Personal Best car in time trial (255 if invalid)
    public byte timeTrialRivalCarIdx; // Index of Rival car in time trial (255 if invalid)
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 50)]
public struct CarLapData
{
    // Times in milliseconds unless specified otherwise
    public uint lastLapTime;
    public uint currentLapTime;
    public ushort sector1TimeMS;
    public byte sector1TimeMinutes; // New to 2023
    public ushort sector2TimeMS;
    public byte sector2TimeMinutes; // New to 2023
    public ushort deltaToCarInFrontMS; // New to 2023
    public ushort deltaToRaceLeaderMS; // New to 2023
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
    public byte totalWarnings;
    public byte cornerCuttingWarnings;
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
