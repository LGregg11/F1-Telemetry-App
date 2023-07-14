namespace F1GameTelemetry.Packets.F12023;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 990)]
public struct FinalClassification
{
    public byte numberCars;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public FinalClassificationData[] finalClassificationData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 45)]
public struct FinalClassificationData
{
    public byte position;
    public byte numberLaps;
    public byte gridPosition;
    public byte points;
    public byte numberPitStops;
    public ResultStatus resultStatus;
    public uint bestLapTime; // In milliseconds
    public double totalRaceTime; // In seconds without penalities
    public byte penalitesTime; // Total in seconds
    public byte numberPenalties;
    public byte numberTyreStints;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public TyreCompoundType[] tyreStintsActual;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public TyreVisualType[] tyreStintsVisual;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] tyreStintsEndLaps; // New to 2022 - The lap number stints end on
}
