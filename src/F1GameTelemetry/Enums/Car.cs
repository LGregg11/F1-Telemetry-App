namespace F1GameTelemetry.Enums;

public enum TyreCompoundType : byte
{
    // F1 Modern
    C5 = 16,
    C4 = 17,
    C3 = 18,
    C2 = 19,
    C1 = 20,
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
    F2Wet = 15
}

public enum TyreVisualType : byte
{
    // F1 Modern & Classic
    Soft = 16,
    Medium = 17,
    Hard = 18,
    Intermediate = 7,
    Wet = 8,

    // F2 '19
    F2Wet = 15,
    F2SuperSoft = 19,
    F2Soft = 20,
    F2Medium = 21,
    F2Hard = 22

}

public enum DrsFault : byte
{
    OK = 0,
    Fault = 1
}

public enum DrsActivation : byte
{
    Off = 0,
    On = 1
}
