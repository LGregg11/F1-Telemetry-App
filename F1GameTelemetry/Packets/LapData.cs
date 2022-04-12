namespace F1GameTelemetry.Packets
{
    using F1GameTelemetry.Packets.Enums;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 946)]
    public struct LapData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarLapData[] carLapData;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 43)]
    public struct CarLapData
    {
        // Times in milliseconds unless specified otherwise
        public uint lastLapTime;

        public uint currentLapTime;

        public ushort sector1Time;

        public ushort sector2Time;

        public float lapDistance; // distance around lap in metres

        public float totalDistance; // metres

        public float safetyCarDelta; // seconds

        public byte carPosition;

        public byte currentlapNum;

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
}
