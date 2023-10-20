namespace F1GameTelemetry.Packets.F12023
{
    using Enums;

    using System.Runtime.InteropServices;


    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1431)]
    public struct SessionHistory
    {
        public byte carIdx;
        public byte numLaps;
        public byte numTyreStints;
        public byte bestLapTimeLapNum;
        public byte bestSector1LapNum;
        public byte bestSector2LapNum;
        public byte bestSector3LapNum;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public LapHistoryData[] lapHistoryData;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public TyreStintHistoryData[] tyreStintHistoryData;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 14)]
    public struct LapHistoryData
    {
        // All times in milliseconds
        public uint lapTime;
        public ushort sector1TimeMS;
        public byte sector1TimeMinutes; // New to 2023
        public ushort sector2TimeMS;
        public byte sector2TimeMinutes; // New to 2023
        public ushort sector3TimeMS;
        public byte sector3TimeMinutes;
        public byte lapValidBitFlags; // 0x01 bit set-lap valid, 0x02 bit set-sector 1 valid, 0x04 bit set-sector 2 valid, 0x08 bit set-sector 3 valid
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
    public struct TyreStintHistoryData
    {
        public byte endLap; // 255 of current tyre
        public TyreCompound tyreActualCompound;
        public TyreVisual tyreVisualCompound;
    }
}
