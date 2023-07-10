namespace F1GameTelemetry.Models;

using Enums;


public struct LapData
{
    public LapData(CarLapData[] carLapData)
    {
        this.carLapData = carLapData;
    }

    public CarLapData[] carLapData;
}

public struct CarLapData
{
    public CarLapData(
        uint lastLapTime,
        uint currentLapTime,
        ushort sector1Time,
        ushort sector2Time,
        float lapDistance,
        byte carPosition,
        byte currentLapNum,
        Sector sector,
        byte currentLapInvalid,
        ResultStatus resultStatus)
    {
        this.lastLapTime = lastLapTime;
        this.currentLapTime = currentLapTime;
        this.sector1Time = sector1Time;
        this.sector2Time = sector2Time;
        this.lapDistance = lapDistance;
        this.carPosition = carPosition;
        this.currentLapNum = currentLapNum;
        this.sector = sector;
        this.currentLapInvalid = currentLapInvalid;
        this.resultStatus = resultStatus;
    }

    // Times in milliseconds unless specified otherwise
    public uint lastLapTime;
    public uint currentLapTime;
    public ushort sector1Time;
    public ushort sector2Time;
    public float lapDistance; // distance around lap in metres
    public byte carPosition;
    public byte currentLapNum;
    public Sector sector;
    public byte currentLapInvalid; // 0 = valid, 1 = invalid
    public ResultStatus resultStatus;
}
