namespace F1GameTelemetry.Packets.F12023;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 201)]
public struct TyreSetsData
{
    public byte carIdx;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public TyreSetData[] tyreSetData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 10)]
public struct TyreSetData
{
    public TyreCompoundType actualTyreCompound;
    public TyreVisualType visualTyreCompound;
    public byte wear; // Percentage
    public byte available;
    public byte recommendedSession;
    public byte lifeSpan; // Laps left on this compound
    public byte usableLife; // Max num laps recommended on this compound
    public short lapDeltaTime;
    public byte fitted;
}