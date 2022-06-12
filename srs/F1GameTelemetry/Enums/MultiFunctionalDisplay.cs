namespace F1GameTelemetry.Enums;

public enum MFDPanelIndexType : byte
{
    Closed = 255,

    // Apparently this might depend on game mode? (Below is for 'single mode')
    CarSetup = 0,
    Pits = 1,
    Damage = 2,
    Engine = 3,
    Temperatures = 4
}

public enum ErsDeploymentMode : byte
{
    None = 0,
    Medium = 1,
    Hotlap = 2,
    Overtake = 3
}

public enum FuelMix : byte
{
    Lean = 0,
    Standard = 1,
    Rich = 2,
    Max = 3
}

