namespace F1GameTelemetry.Models;

using Enums;


public struct SessionHistory
{
    public SessionHistory(
        byte carIdx,
        byte numLaps,
        byte numTyreStints,
        byte bestLapTimeLapNum,
        byte bestSector1LapNum,
        byte bestSector2LapNum,
        byte bestSector3LapNum,
        LapHistoryData[] lapHistoryData,
        TyreStintHistoryData[] tyreStintHistoryData)
    {
        this.carIdx = carIdx;
        this.numLaps = numLaps;
        this.numTyreStints = numTyreStints;
        this.bestLapTimeLapNum = bestLapTimeLapNum;
        this.bestSector1LapNum = bestSector1LapNum;
        this.bestSector2LapNum = bestSector2LapNum;
        this.bestSector3LapNum = bestSector3LapNum;
        this.lapHistoryData = lapHistoryData;
        this.tyreStintHistoryData = tyreStintHistoryData;
    }

    public byte carIdx;
    public byte numLaps;
    public byte numTyreStints;
    public byte bestLapTimeLapNum;
    public byte bestSector1LapNum;
    public byte bestSector2LapNum;
    public byte bestSector3LapNum;
    public LapHistoryData[] lapHistoryData;
    public TyreStintHistoryData[] tyreStintHistoryData;

}

public struct LapHistoryData
{
    public LapHistoryData(
        uint lapTime,
        ushort sector1Time,
        ushort sector2Time,
        ushort sector3Time)
    {
        this.lapTime = lapTime;
        this.sector1Time = sector1Time;
        this.sector2Time = sector2Time;
        this.sector3Time = sector3Time;
    }

    // All times in milliseconds
    public uint lapTime;
    public ushort sector1Time;
    public ushort sector2Time;
    public ushort sector3Time;
}

public struct TyreStintHistoryData
{
    public TyreStintHistoryData(
        byte endLap,
        TyreVisualType tyreVisualCompound)
    {
        this.endLap = endLap;
        this.tyreVisualCompound = tyreVisualCompound;
    }

    public byte endLap; // 255 of current tyre
    public TyreVisualType tyreVisualCompound;
}
