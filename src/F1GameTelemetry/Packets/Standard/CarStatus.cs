namespace F1GameTelemetry.Packets.Standard;

using F1GameTelemetry.Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1034)]
public struct CarStatus
{
    public CarStatus(CarStatusData[] carStatusData)
    {
        this.carStatusData = carStatusData;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarStatusData[] carStatusData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 47)]
public struct CarStatusData
{
    public CarStatusData(
        TractionControlType trackionControl,
        FuelMix antiLockBrakes,
        byte fuelMix,
        byte frontBrakeBias,
        byte pitLimiterStatus,
        float fuelInTank,
        float fuelCapacity,
        float fuelRemainingLaps,
        ushort maxRPM,
        ushort idleRPM,
        byte maxGears,
        byte drsAllowed,
        ushort drsActivationDistance,
        TyreCompoundType actualTyreCompound,
        TyreVisualType visualTyreCompound,
        byte tyresAgeLaps,
        FiaFlagType vehicleFiaFlags,
        float ersStoreEnergy,
        ErsDeploymentMode ersDeployMode,
        float ersHarvestedThisLapMGUK,
        float ersHarvestedThisLapMGUH,
        float ersDeployedThisLap,
        byte networkPaused)
    {
        this.trackionControl = trackionControl;
        this.antiLockBrakes = antiLockBrakes;
        this.fuelMix = fuelMix;
        this.frontBrakeBias = frontBrakeBias;
        this.pitLimiterStatus = pitLimiterStatus;
        this.fuelInTank = fuelInTank;
        this.fuelCapacity = fuelCapacity;
        this.fuelRemainingLaps = fuelRemainingLaps;
        this.maxRPM = maxRPM;
        this.idleRPM = idleRPM;
        this.maxGears = maxGears;
        this.drsAllowed = drsAllowed;
        this.drsActivationDistance = drsActivationDistance;
        this.actualTyreCompound = actualTyreCompound;
        this.visualTyreCompound = visualTyreCompound;
        this.tyresAgeLaps = tyresAgeLaps;
        this.vehicleFiaFlags = vehicleFiaFlags;
        this.ersStoreEnergy = ersStoreEnergy;
        this.ersDeployMode = ersDeployMode;
        this.ersHarvestedThisLapMGUK = ersHarvestedThisLapMGUK;
        this.ersHarvestedThisLapMGUH = ersHarvestedThisLapMGUH;
        this.ersDeployedThisLap = ersDeployedThisLap;
        this.networkPaused = networkPaused;
    }

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
    public TyreCompoundType actualTyreCompound;
    public TyreVisualType visualTyreCompound;
    public byte tyresAgeLaps;
    public FiaFlagType vehicleFiaFlags;
    public float ersStoreEnergy;
    public ErsDeploymentMode ersDeployMode;
    public float ersHarvestedThisLapMGUK;
    public float ersHarvestedThisLapMGUH;
    public float ersDeployedThisLap;
    public byte networkPaused;
}
