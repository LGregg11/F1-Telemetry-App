﻿namespace F1GameTelemetry.Packets.F12021;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 5)]
public struct FastestLap
{
    public byte vehicleIdx;
    public float lapTime; // Lap time in seconds
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct Retirement
{
    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct TeamMateInPits
{
    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct RaceWinner
{
    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 7)]
public struct Penalty
{
    public byte penaltyType;
    public byte infringementType;
    public byte vehicleIdx; // Vehicle index of the car with the penalty
    public byte otherVehicleIdx; // Vehicle index of the other car involved
    public byte time; // Time gained, or time spent doing action in seconds
    public byte lapNum;
    public byte placesGained;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 7)]
public struct SpeedTrap
{
    public byte vehicleIdx;
    public float speed; // Top speed reached in kilometres per hour
    public byte overallFastestInSession; // Overall fastest in session = 1, otherwise 0
    public byte driverFastestInSession; // Fastest speed for driver in session = 1, otherwise 0
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct StartLights
{
    public byte numLights;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct DriveThroughPenaltyServed
{
    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct StopGoPenaltyServed
{
    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public struct Flashback
{
    public uint flashbackFrameIdentifier; // Frame identifier flashed back to
    public float flashbackSessionTime; // Session time flashed back to
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct Buttons
{
    public byte buttonStatus; // Bit flags specifying which buttons are being pressed currently
}
