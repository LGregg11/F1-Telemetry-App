namespace F1GameTelemetry.Packets.F12021;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Listener;
using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 814)]
public struct FinalClassification
{
    public byte numberCars;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public FinalClassificationData[] finalClassificationData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 37)]
public struct FinalClassificationData
{
    public byte position;

    public byte numberLaps;

    public byte gridPosition;

    public byte points;

    public byte numberPitStops;

    public ResultStatus resultStatus;

    public uint bestLapTime; // In milliseconds

    public double totalRaceTime; // In seconds without penalities

    public byte penalitesTime; // Total in seconds

    public byte numberPenalties;

    public byte numberTyreStints;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public TyreCompoundType[] tyreStintsActual; // Max expected length = 8

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public TyreVisualType[] tyreStintsVisual; // Max expected length = 8
}

public class FinalClassificationPacket : IPacket
{
    public event EventHandler? Received;

    public void ReceivePacket(byte[] remainingPacket)
    {
        var args = new FinalClassificationEventArgs
        {
            FinalClassification = Converter.BytesToPacket<FinalClassification>(remainingPacket)
        };

        Received?.Invoke(this, args);
    }
}
