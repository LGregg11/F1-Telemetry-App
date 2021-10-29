namespace F1GameTelemetry.Packets
{
    using F1GameTelemetry.Packets.Enums;
    using System.Runtime.InteropServices;

    public struct CarStatus
    {
        public CarStatusData[] carStatusData;
    }

    [StructLayout(LayoutKind.Explicit, Size = 47)]
    public struct CarStatusData
    {
        [FieldOffset(0)]
        public TractionControlTypes trackionControl;

        [FieldOffset(1)]
        public FuelMix antiLockBrakes;

        [FieldOffset(2)]
        public byte fuelMix;

        [FieldOffset(3)]
        public byte frontBrakeBias;

        [FieldOffset(4)]
        public byte pitLimiterStatus;

        [FieldOffset(5)]
        public float fuelInTank;

        [FieldOffset(9)]
        public float fuelCapacity;

        [FieldOffset(13)]
        public float fuelRemainingLaps;

        [FieldOffset(17)]
        public ushort maxRPM;

        [FieldOffset(19)]
        public ushort idleRPM;

        [FieldOffset(21)]
        public byte maxGears;

        [FieldOffset(22)]
        public byte drsAllowed;

        [FieldOffset(23)]
        public ushort drsActivationDistance;

        [FieldOffset(25)]
        public TyreCompoundTypes actualTyreCompound;

        [FieldOffset(26)]
        public TyreVisualTypes visualTyreCompound;

        [FieldOffset(27)]
        public byte tyresAgeLaps;

        [FieldOffset(28)]
        public FiaFlagTypes vehicleFiaFlags;

        [FieldOffset(29)]
        public float ersStoreEnergy;

        [FieldOffset(33)]
        public ErsDeploymentModes ersDeployMode;

        [FieldOffset(34)]
        public float ersHarvestedThisLapMGUK;

        [FieldOffset(38)]
        public float ersHarvestedThisLapMGUH;

        [FieldOffset(42)]
        public float ersDeployedThisLap;

        [FieldOffset(46)]
        public byte networkPaused;
    }
}
