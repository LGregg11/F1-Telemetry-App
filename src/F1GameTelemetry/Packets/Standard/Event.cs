namespace F1GameTelemetry.Packets.Standard;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 5)]
public struct FastestLap
{
    public FastestLap(byte vehicleIdx, float lapTime)
    {
        this.vehicleIdx = vehicleIdx;
        this.lapTime = lapTime;
    }

    public byte vehicleIdx;
    public float lapTime; // Lap time in seconds
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct Retirement
{
    public Retirement(byte vehicleIdx)
    {
        this.vehicleIdx = vehicleIdx;
    }

    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct TeamMateInPits
{
    public TeamMateInPits(byte vehicleIdx)
    {
        this.vehicleIdx = vehicleIdx;
    }

    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct RaceWinner
{
    public RaceWinner(byte vehicleIdx)
    {
        this.vehicleIdx = vehicleIdx;
    }

    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 7)]
public struct Penalty
{
    public Penalty(byte penaltyType, byte infringementType, byte vehicleIdx, byte otherVehicleIdx, byte time, byte lapNum, byte placesGained)
    {
        this.penaltyType = penaltyType;
        this.infringementType = infringementType;
        this.vehicleIdx = vehicleIdx;
        this.otherVehicleIdx = otherVehicleIdx;
        this.time = time;
        this.lapNum = lapNum;
        this.placesGained = placesGained;
    }

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
    public SpeedTrap(byte vehicleIdx, float speed, byte overallFastestInSession, byte driverFastestInSession)
    {
        this.vehicleIdx = vehicleIdx;
        this.speed = speed;
        this.overallFastestInSession = overallFastestInSession;
        this.driverFastestInSession = driverFastestInSession;
    }

    public byte vehicleIdx;
    public float speed; // Top speed reached in kilometres per hour
    public byte overallFastestInSession; // Overall fastest in session = 1, otherwise 0
    public byte driverFastestInSession; // Fastest speed for driver in session = 1, otherwise 0
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct StartLights
{
    public StartLights(byte numLights)
    {
        this.numLights = numLights;
    }

    public byte numLights;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct DriveThroughPenaltyServed
{
    public DriveThroughPenaltyServed(byte vehicleIdx)
    {
        this.vehicleIdx = vehicleIdx;
    }

    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct StopGoPenaltyServed
{
    public StopGoPenaltyServed(byte vehicleIdx)
    {
        this.vehicleIdx = vehicleIdx;
    }

    public byte vehicleIdx;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public struct Flashback
{
    public Flashback(uint flashbackFrameIdentifier, float flashbackSessionTime)
    {
        this.flashbackFrameIdentifier = flashbackFrameIdentifier;
        this.flashbackSessionTime = flashbackSessionTime;
    }

    public uint flashbackFrameIdentifier; // Frame identifier flashed back to
    public float flashbackSessionTime; // Session time flashed back to
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
public struct Buttons
{
    public Buttons(byte buttonStatus)
    {
        this.buttonStatus = buttonStatus;
    }

    public byte buttonStatus; // Bit flags specifying which buttons are being pressed currently
}
