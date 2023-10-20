namespace F1GameTelemetry.Enums
{
    using System.ComponentModel;

    public enum TyreCompound : byte
    {
        // F1 Modern
        C5 = 16,
        C4 = 17,
        C3 = 18,
        C2 = 19,
        C1 = 20,
        C0 = 21, // New to 2023
        Intermediate = 7,
        Wet = 8,

        // F1 Classic
        ClassicDry = 9,
        ClassicWet = 10,

        // F2
        F2SuperSoft = 11,
        F2Soft = 12,
        F2Medium = 13,
        F2Hard = 14,
        F2Wet = 15,
    }

    public enum TyreVisual : byte
    {
        // F1 Modern & Classic
        [Description("S")]
        Soft = 16,

        [Description("M")]
        Medium = 17,

        [Description("H")]
        Hard = 18,

        [Description("I")]
        Intermediate = 7,

        [Description("W")]
        Wet = 8,

        // F2 '19
        F2Wet = 15,
        F2SuperSoft = 19,
        F2Soft = 20,
        F2Medium = 21,
        F2Hard = 22
    }

    public enum Fault : byte
    {
        OK = 0,
        Fault = 1
    }

    public enum DrsActivation : byte
    {
        Off = 0,
        On = 1
    }
}
