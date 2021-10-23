namespace F1GameTelemetry.Packets
{
    using System.Runtime.InteropServices;

    #region Event Structs

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 5)]
    public struct FastestLap
    {
        [FieldOffset(0)]
        public byte vehicleIdx;

        [FieldOffset(1)]
        public float lapTime; // Lap time in seconds
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 1)]
    public struct Retirement
    {
        [FieldOffset(0)]
        public byte vehicleIdx;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 1)]
    public struct TeamMateInPits
    {
        [FieldOffset(0)]
        public byte vehicleIdx;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 1)]
    public struct RaceWinner
    {
        [FieldOffset(0)]
        public byte vehicleIdx;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 7)]
    public struct Penalty
    {
        [FieldOffset(0)]
        public byte penaltyType;

        [FieldOffset(1)]
        public byte infringementType;

        [FieldOffset(2)]
        public byte vehicleIdx; // Vehicle index of the car with the penalty

        [FieldOffset(3)]
        public byte otherVehicleIdx; // Vehicle index of the other car involved

        [FieldOffset(4)]
        public byte time; // Time gained, or time spent doing action in seconds

        [FieldOffset(5)]
        public byte lapNum;

        [FieldOffset(6)]
        public byte placesGained;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 7)]
    public struct SpeedTrap
    {
        [FieldOffset(0)]
        public byte vehicleIdx;

        [FieldOffset(1)]
        public float speed; // Top speed reached in kilometres per hour

        [FieldOffset(5)]
        public byte overallFastestInSession; // Overall fastest in session = 1, otherwise 0

        [FieldOffset(6)]
        public byte driverFastestInSession; // Fastest speed for driver in session = 1, otherwise 0
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 1)]
    public struct StartLights
    {
        [FieldOffset(0)]
        public byte numLights;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 1)]
    public struct DriveThroughPenaltyServed
    {
        [FieldOffset(0)]
        public byte vehicleIdx;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 1)]
    public struct StopGoPenaltyServed
    {
        [FieldOffset(0)]
        public byte vehicleIdx;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 8)]
    public struct Flashback
    {
        [FieldOffset(0)]
        public uint flashbackFrameIdentifier; // Frame identifier flashed back to

        [FieldOffset(4)]
        public float flashbackSessionTime; // Session time flashed back to
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 1)]
    public struct Buttons
    {
        [FieldOffset(0)]
        public byte buttonStatus; // Bit flags specifying which buttons are being pressed currently
    }
    #endregion
}
