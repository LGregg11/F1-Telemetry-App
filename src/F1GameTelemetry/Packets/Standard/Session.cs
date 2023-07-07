namespace F1GameTelemetry.Packets.Standard;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 5)]
public struct MarshalZones
{
    public MarshalZones(float zoneStart, ZoneFlag zoneFlag)
    {
        this.zoneStart = zoneStart;
        this.zoneFlag = zoneFlag;
    }

    public float zoneStart; // Fraction (0..1) of way through the lap the marshal zone starts
    public ZoneFlag zoneFlag;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public struct WeatherForecastSample
{
    public WeatherForecastSample(
        SessionType sessionType,
        byte timeOffset,
        Weather weather,
        sbyte trackTemperature,
        TemperatureChange trackTemperatureChange,
        sbyte airTemperature,
        TemperatureChange airTemperatureChange,
        byte rainPercentage)
    {
        this.sessionType = sessionType;
        this.timeOffset = timeOffset;
        this.weather = weather;
        this.trackTemperature = trackTemperature;
        this.trackTemperatureChange = trackTemperatureChange;
        this.airTemperature = airTemperature;
        this.airTemperatureChange = airTemperatureChange;
        this.rainPercentage = rainPercentage;

    }

    public SessionType sessionType;
    public byte timeOffset; // Time in minutes the forecast is for
    public Weather weather;
    public sbyte trackTemperature; // Celcius
    public TemperatureChange trackTemperatureChange;
    public sbyte airTemperature; // Celcius
    public TemperatureChange airTemperatureChange;
    public byte rainPercentage;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 601)]
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
        Formula formula,
        ushort sessionTimeLeft,
        ushort sessionDuration,
        byte pitSpeedLimit,
        byte gamePaused,
        byte isSpectating,
        byte spectatorCarIndex,
        byte sliPriNativeSupport,
        byte numMarshalZones,
        MarshalZones[] marshalZones,
        SafetyCarStatus safetyCarStatus,
        byte networkGame,
        byte numWeatherForecastSamples,
        WeatherForecastSample[] weatherForecastSamples,
        byte forecastAccuracy,
        byte aiDifficulty,
        uint seasonLinkIdentifier,
        uint weekendLinkIdentifier,
        uint sessionLinkIdentifier,
        byte pitStopWindowIdealLap,
        byte pitStopWindowLatestLap,
        byte pitStopRejoinPosition,
        BasicAssist steeringAssist,
        BrakingAssist brakingAssist,
        GearboxAssist gearboxAssist,
        BasicAssist pitAssist,
        BasicAssist pitReleaseAssist,
        BasicAssist ERSAssist,
        BasicAssist DRSAssist,
        DynamicRacingLine dynamicRacingLine,
        DynamicRacingLineType dynamicRacingLineType
        )
    {
        this.weather = weather;
        this.trackTemperature = trackTemperature;
        this.airTemperature = airTemperature;
        this.totalLaps = totalLaps;
        this.trackLength = trackLength;
        this.sessionType = sessionType;
        this.trackId = trackId;
        this.formula = formula;
        this.sessionTimeLeft = sessionTimeLeft;
        this.sessionDuration = sessionDuration;
        this.pitSpeedLimit = pitSpeedLimit;
        this.gamePaused = gamePaused;
        this.isSpectating = isSpectating;
        this.spectatorCarIndex = spectatorCarIndex;
        this.sliPriNativeSupport = sliPriNativeSupport;
        this.numMarshalZones = numMarshalZones;
        this.marshalZones = marshalZones;
        this.safetyCarStatus = safetyCarStatus;
        this.networkGame = networkGame;
        this.numWeatherForecastSamples = numWeatherForecastSamples;
        this.weatherForecastSamples = weatherForecastSamples;
        this.forecastAccuracy = forecastAccuracy;
        this.aiDifficulty = aiDifficulty;
        this.seasonLinkIdentifier = seasonLinkIdentifier;
        this.weekendLinkIdentifier = weekendLinkIdentifier;
        this.sessionLinkIdentifier = sessionLinkIdentifier;
        this.pitStopWindowIdealLap = pitStopWindowIdealLap;
        this.pitStopWindowLatestLap = pitStopWindowLatestLap;
        this.pitStopRejoinPosition = pitStopRejoinPosition;
        this.steeringAssist = steeringAssist;
        this.brakingAssist = brakingAssist;
        this.gearboxAssist = gearboxAssist;
        this.pitAssist = pitAssist;
        this.pitReleaseAssist = pitReleaseAssist;
        this.ERSAssist = ERSAssist;
        this.DRSAssist = DRSAssist;
        this.dynamicRacingLine = dynamicRacingLine;
        this.dynamicRacingLineType = dynamicRacingLineType;
    }

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
}
