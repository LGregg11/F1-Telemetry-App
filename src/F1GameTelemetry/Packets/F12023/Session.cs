﻿namespace F1GameTelemetry.Packets.F12023;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 615)]
public struct Session
{
    public Weather weather;
    public sbyte trackTemperature; // Celsius
    public sbyte airTemperature; // Celsius
    public byte totalLaps;
    public ushort trackLength; // metres
    public SessionType sessionType;
    public Track trackId;
    public Formula formula;
    public ushort sessionTimeLeft; // seconds
    public ushort sessionDuration; // seconds
    public byte pitSpeedLimit; // kph
    public byte gamePaused;
    public byte isSpectating;
    public byte spectatorCarIndex;
    public byte sliPriNativeSupport; // 0 = inactive, 1 = active
    public byte numMarshalZones;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
    public MarshalZones[] marshalZones;

    public SafetyCarStatus safetyCarStatus;
    public byte networkGame; // 0 = offline, 1 = online
    public byte numWeatherForecastSamples; // Amount of weatherForecastSamples

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
    public WeatherForecastSample[] weatherForecastSamples;

    public byte forecastAccuracy; // 0 = Perfect, 1 = Approximate
    public byte aiDifficulty; // 0-110
    public uint seasonLinkIdentifier;
    public uint weekendLinkIdentifier;
    public uint sessionLinkIdentifier;
    public byte pitStopWindowIdealLap;
    public byte pitStopWindowLatestLap;
    public byte pitStopRejoinPosition;
    public BasicAssist steeringAssist;
    public BrakingAssist brakingAssist;
    public GearboxAssist gearboxAssist;
    public BasicAssist pitAssist;
    public BasicAssist pitReleaseAssist;
    public BasicAssist ERSAssist;
    public BasicAssist DRSAssist;
    public DynamicRacingLine dynamicRacingLine;
    public DynamicRacingLineType dynamicRacingLineType;
    public GameMode gameMode; // New to 2022
    public RuleSet ruleSet; // New to 2022
    public uint timeOfDay; //  New to 2022 - Local - time since midnight
    public SessionLength sessionLength; // New to 2022
    public SpeedUnit speedUnitsLeadPlayer; // New to 2023
    public TemperatureUnit temperatureUnitsLeadPlayer; // New to 2023
    public SpeedUnit speedUnitsSecondaryPlayer; // New to 2023
    public TemperatureUnit temperatureUnitsSecondaryPlayer; // New to 2023
    public byte numSafetyCarPeriods; // New to 2023
    public byte numVirtualSafetyCarPeriods; // New to 2023
    public byte numRedFlagPeriods; // New to 2023
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 5)]
public struct MarshalZones
{
    public float zoneStart; // Fraction (0..1) of way through the lap the marshal zone starts
    public ZoneFlag zoneFlag;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public struct WeatherForecastSample
{
    public SessionType sessionType;
    public byte timeOffset; // Time in minutes the forecast is for
    public Weather weather;
    public sbyte trackTemperature; // Celcius
    public TemperatureChange trackTemperatureChange;
    public sbyte airTemperature; // Celcius
    public TemperatureChange airTemperatureChange;
    public byte rainPercentage;
}
