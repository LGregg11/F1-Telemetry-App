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
        this.bestSectorTimeLapNums = new byte[3]
        {
            bestSector1LapNum,
            bestSector2LapNum,
            bestSector3LapNum
        };
        this.lapHistoryData = lapHistoryData;
        this.tyreStintHistoryData = tyreStintHistoryData;
    }

    public byte carIdx;
    public byte numLaps;
    public byte numTyreStints;
    public byte bestLapTimeLapNum;
    public byte[] bestSectorTimeLapNums;
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
        this.sectorTimes = new ushort[3]
        {
            sector1Time,
            sector2Time,
            sector3Time
        };
    }

    // All times in milliseconds
    public uint lapTime;
    public ushort[] sectorTimes;
}

public struct TyreStintHistoryData
{
    public TyreStintHistoryData(
        byte endLap,
        TyreVisual tyreVisualCompound)
    {
        this.endLap = endLap;
        this.tyreVisualCompound = tyreVisualCompound;
    }

    public byte endLap; // 255 of current tyre
    public TyreVisual tyreVisualCompound;
}
