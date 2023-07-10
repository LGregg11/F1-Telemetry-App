namespace F1GameTelemetry.Models;

using Enums;


public struct FinalClassification
{
    public FinalClassification(byte numberCars, FinalClassificationData[] finalClassificationData)
    {
        this.numberCars = numberCars;
        this.finalClassificationData = finalClassificationData;
    }

    public byte numberCars;
    public FinalClassificationData[] finalClassificationData;
}

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
    public TyreVisualType[] tyreStintsVisual;
}
