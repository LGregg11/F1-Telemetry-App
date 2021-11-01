namespace F1GameTelemetry.Packets
{
    using F1GameTelemetry.Packets.Enums;
    using System.Runtime.InteropServices;

    public struct FinalClassification
    {
        public byte numberCars;
        public FinalClassificationData[] finalClassificationData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 37)]
    public struct FinalClassificationData
    {
        public byte position;

        public byte numberLaps;

        public byte gridPosition;

        public byte points;

        public byte numberPitStops;

        public ResultStatusTypes resultStatus;

        public uint bestLapTime; // In milliseconds

        public double totalRaceTime; // In seconds without penalities

        public byte penalitesTime; // Total in seconds

        public byte numberPenalties;

        public byte numberTyreStints;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public TyreCompoundTypes[] tyreStintsActual; // Max expected length = 8

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public TyreVisualTypes[] tyreStintsVisual; // Max expected length = 8
    }
}
