namespace F1GameTelemetry.Packets.Standard;

using F1GameTelemetry.Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1131)]
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

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public LapHistoryData[] lapHistoryData;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public TyreStintHistoryData[] tyreStintHistoryData;

}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 11)]
public struct LapHistoryData
{
    public LapHistoryData(
        uint lapTime,
        ushort sector1Time,
        ushort sector2Time,
        ushort sector3Time,
        byte lapValidBitFlags)
    {
        this.lapTime = lapTime;
        this.sector1Time = sector1Time;
        this.sector2Time = sector2Time;
        this.sector3Time = sector3Time;
        this.lapValidBitFlags = lapValidBitFlags;
    }

    // All times in milliseconds
    public uint lapTime;
    public ushort sector1Time;
    public ushort sector2Time;
    public ushort sector3Time;
    public byte lapValidBitFlags; // 0x01 bit set-lap valid, 0x02 bit set-sector 1 valid, 0x04 bit set-sector 2 valid, 0x08 bit set-sector 3 valid
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
public struct TyreStintHistoryData
{
    public TyreStintHistoryData(
        byte endLap,
        TyreCompoundType tyreActualCompound,
        TyreVisualType tyreVisualCompound)
    {
        this.endLap = endLap;
        this.tyreActualCompound = tyreActualCompound;
        this.tyreVisualCompound = tyreVisualCompound;
    }

    public byte endLap; // 255 of current tyre
    public TyreCompoundType tyreActualCompound;
    public TyreVisualType tyreVisualCompound;
}
