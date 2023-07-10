namespace F1GameTelemetry.Models;

using Enums;


public struct CarTelemetry
{
    public CarTelemetry(CarTelemetryData[] carTelemetryData)
    {
        this.carTelemetryData = carTelemetryData;
    }

    public CarTelemetryData[] carTelemetryData;
}

public struct CarTelemetryData
{
    public CarTelemetryData(
        ushort speed,
        float throttle,
        float steer,
        float brake,
        sbyte gear,
        DrsActivation drs,
        FourAxleUnsignedShort brakesTemperature,
        FourAxleByte tyresSurfaceTemperature,
        FourAxleByte tyresInnerTemperature)
    {
        this.speed = speed;
        this.throttle = throttle;
        this.steer = steer;
        this.brake = brake;
        this.gear = gear;
        this.drs = drs;
        this.brakesTemperature = brakesTemperature;
        this.tyresSurfaceTemperature = tyresSurfaceTemperature;
        this.tyresInnerTemperature = tyresInnerTemperature;
    }

    public ushort speed; // km/h
    public float throttle; // 0.0 - 1.0
    public float steer; // -1.0 (full lock left) - 1.0 (full lock right)
    public float brake; // 0.0 - 1.0
    public sbyte gear; // Gears=1-8, Neutral=0, Reverse=-1
    public DrsActivation drs;
    public FourAxleUnsignedShort brakesTemperature;
    public FourAxleByte tyresSurfaceTemperature;
    public FourAxleByte tyresInnerTemperature;
}
