namespace F1GameTelemetry.Enums;

public enum Weather : byte
{
    Unknown = 255,

    Clear = 0,
    LightCloud = 1,
    Overcast = 2,
    LightRain = 3,
    HeavyRain = 4,
    Storm = 5
}

public enum TemperatureChange : sbyte
{
    Up = 0,
    Down = 1,
    NoChange = 2
}

public enum Formula : byte
{
    F1Modern = 0,
    F1Classic = 1,
    F2 = 2,
    F1Generic = 3
}

public enum SessionType : byte
{
    Unknown = 0,
    P1 = 1,
    P2 = 2,
    P3 = 3,
    ShortP = 4,
    Q1 = 5,
    Q2 = 6,
    Q3 = 7,
    ShortQ = 8,
    OSQ = 9,
    R = 10,
    R2 = 11,
    R3 = 12,
    TimeTrial = 13
}

public enum SafetyCarStatus : byte
{
    None = 0,
    Full = 1,
    Virtual = 2,
    FormationLap = 3
}
