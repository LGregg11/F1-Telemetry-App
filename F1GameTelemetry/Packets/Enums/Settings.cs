namespace F1GameTelemetry.Packets.Enums
{
    public enum UdpSetting : byte
    {
        Restricted = 0,
        Public = 1
    }

    public enum TractionControlType : byte
    {
        Off = 0,
        Medium = 1,
        Full = 2
    }

    public enum BasicAssist : byte
    {
        Off = 0,
        On = 1
    }

    public enum BrakingAssist : byte
    {
        Off = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum GearboxAssist : byte
    {
        Manual = 1,
        ManualAndSuggested = 2,
        Auto = 3
    }

    public enum DynamicRacingLine : byte
    {
        Off = 0,
        CornersOnly = 1,
        Full = 2
    }

    public enum DynamicRacingLineType : byte
    {
        TwoD = 0,
        ThreeD = 1
    }
}
