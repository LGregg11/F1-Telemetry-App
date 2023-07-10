namespace F1GameTelemetry.Models;

using Enums;


public struct Session
{
    public Session(
        Weather weather,
        sbyte trackTemperature,
        sbyte airTemperature,
        byte totalLaps,
        ushort trackLength,
        SessionType sessionType,
        Track trackId,
        ushort sessionDuration,
        SafetyCarStatus safetyCarStatus,
        byte numWeatherForecastSamples,
        WeatherForecastSample[] weatherForecastSamples,
        byte pitStopWindowIdealLap,
        byte pitStopWindowLatestLap,
        byte pitStopRejoinPosition
        )
    {
        this.weather = weather;
        this.trackTemperature = trackTemperature;
        this.airTemperature = airTemperature;
        this.totalLaps = totalLaps;
        this.trackLength = trackLength;
        this.sessionType = sessionType;
        this.trackId = trackId;
        this.sessionDuration = sessionDuration;
        this.safetyCarStatus = safetyCarStatus;
        this.numWeatherForecastSamples = numWeatherForecastSamples;
        this.weatherForecastSamples = weatherForecastSamples;
        this.pitStopWindowIdealLap = pitStopWindowIdealLap;
        this.pitStopWindowLatestLap = pitStopWindowLatestLap;
        this.pitStopRejoinPosition = pitStopRejoinPosition;
    }

    public Weather weather;
    public sbyte trackTemperature; // Celsius
    public sbyte airTemperature; // Celsius
    public byte totalLaps;
    public ushort trackLength; // metres
    public SessionType sessionType;
    public Track trackId;
    public ushort sessionDuration; // seconds
    public SafetyCarStatus safetyCarStatus;
    public byte numWeatherForecastSamples;
    public WeatherForecastSample[] weatherForecastSamples;

    public byte pitStopWindowIdealLap;
    public byte pitStopWindowLatestLap;
    public byte pitStopRejoinPosition;
}

public struct WeatherForecastSample
{
    public WeatherForecastSample(
        SessionType sessionType,
        byte timeOffset,
        Weather weather,
        sbyte trackTemperature,
        byte rainPercentage)
    {
        this.sessionType = sessionType;
        this.timeOffset = timeOffset;
        this.weather = weather;
        this.trackTemperature = trackTemperature;
        this.rainPercentage = rainPercentage;

    }

    public SessionType sessionType;
    public byte timeOffset; // Time in minutes the forecast is for
    public Weather weather;
    public sbyte trackTemperature; // Celcius
    public byte rainPercentage;
}