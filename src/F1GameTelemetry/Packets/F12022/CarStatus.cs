﻿namespace F1GameTelemetry.Packets.F12022;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1034)]
public struct CarStatus
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarStatusData[] carStatusData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 47)]
public struct CarStatusData
{
    public TractionControlType trackionControl;
    public FuelMix antiLockBrakes;
    public byte fuelMix;
    public byte frontBrakeBias;
    public byte pitLimiterStatus;
    public float fuelInTank;
    public float fuelCapacity;
    public float fuelRemainingLaps;
    public ushort maxRPM;
    public ushort idleRPM;
    public byte maxGears;
    public byte drsAllowed;
    public ushort drsActivationDistance;
    public TyreCompound actualTyreCompound;
    public TyreVisual visualTyreCompound;
    public byte tyresAgeLaps;
    public FiaFlagType vehicleFiaFlags;
    public float ersStoreEnergy;
    public ErsDeploymentMode ersDeployMode;
    public float ersHarvestedThisLapMGUK;
    public float ersHarvestedThisLapMGUH;
    public float ersDeployedThisLap;
    public byte networkPaused;
}
