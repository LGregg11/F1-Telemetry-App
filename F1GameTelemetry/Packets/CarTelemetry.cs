namespace F1GameTelemetry.Packets
{
    using F1GameTelemetry.Packets.Enums;
    using System.Runtime.InteropServices;

    public struct CarTelemetry
    {
        public CarTelemetryData[] carTelemetryData;

        public MFDPanelIndexTypes mfdPanelIndex;

        public MFDPanelIndexTypes mfdPanelIndexSecondaryPlayer;

        public int suggestedGear; // 1-8 (0 if no gear suggested)
    }

    [StructLayout(LayoutKind.Explicit, Size = 60)]
    public struct CarTelemetryData
    {
        [FieldOffset(0)]
        public ushort speed; // km/h

        [FieldOffset(2)]
        public float throttle; // 0.0 - 1.0

        [FieldOffset(6)]
        public float steer; // -1.0 (full lock left) - 1.0 (full lock right)

        [FieldOffset(10)]
        public float brake; // 0.0 - 1.0

        [FieldOffset(14)]
        public byte clutch; // 0 - 100

        [FieldOffset(15)]
        public sbyte gear; // Gears=1-8 Neutral=0, Reverse=-1

        [FieldOffset(16)]
        public ushort engineRPM;

        [FieldOffset(18)]
        public byte drs; // 0 = Off, 1 = On

        [FieldOffset(19)]
        public byte revLightsPercent;

        [FieldOffset(20)]
        public ushort revLightsBitValue; // bit 0 = leftmost LED, bit 14 = rightmost LED

        [FieldOffset(22)]
        public BrakeData brakesTemperature;

        [FieldOffset(30)]
        public TyreTemperatureData tyresSurfaceTemperature;

        [FieldOffset(34)]
        public TyreTemperatureData tyresInnerTemperature;

        [FieldOffset(38)]
        public ushort engineTemperature;

        [FieldOffset(40)]
        public TyrePressureData tyrePressure;

        [FieldOffset(56)]
        public SurfaceTypeData surfaceType;
    }
}
