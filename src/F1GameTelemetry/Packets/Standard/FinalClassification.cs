namespace F1GameTelemetry.Packets.Standard;

using F1GameTelemetry.Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 814)]
public struct FinalClassification
{
    public FinalClassification(byte numberCars, FinalClassificationData[] finalClassificationData)
    {
        this.numberCars = numberCars;
        this.finalClassificationData = finalClassificationData;
    }

    public byte numberCars;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public FinalClassificationData[] finalClassificationData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 37)]
public struct FinalClassificationData
{
    public FinalClassificationData(
        byte position,
        byte numberLaps,
        byte gridPosition,
        byte points,
        byte numberPitStops,
        ResultStatus resultStatus,
        uint bestLapTime,
        double totalRaceTime,
        byte penalitesTime,
        byte numberPenalties,
        byte numberTyreStints,
        TyreCompoundType[] tyreStintsActual,
        TyreVisualType[] tyreStintsVisual)
    {
        this.position = position;
        this.numberLaps = numberLaps;
        this.gridPosition = gridPosition;
        this.points = points;
        this.numberPitStops = numberPitStops;
        this.resultStatus = resultStatus;
        this.bestLapTime = bestLapTime;
        this.totalRaceTime = totalRaceTime;
        this.penalitesTime = penalitesTime;
        this.numberPenalties = numberPenalties;
        this.numberTyreStints = numberTyreStints;
        this.tyreStintsActual = tyreStintsActual;
        this.tyreStintsVisual = tyreStintsVisual;
    }

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
    public TyreCompoundType[] tyreStintsActual; // Max expected length = 8

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public TyreVisualType[] tyreStintsVisual; // Max expected length = 8
}
