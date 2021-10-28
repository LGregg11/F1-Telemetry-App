namespace F1GameTelemetry.Packets
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct VectorData
    {
        [FieldOffset(0)]
        public float x;

        [FieldOffset(4)]
        public float y;

        [FieldOffset(8)]
        public float z;
    }

    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public struct NormalisedVectorData
    {
        [FieldOffset(0)]
        public short x;

        [FieldOffset(2)]
        public short y;

        [FieldOffset(4)]
        public short z;
    }

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct CoordinateData
    {
        [FieldOffset(0)]
        public float lateral;

        [FieldOffset(4)]
        public float longitudal;

        [FieldOffset(8)]
        public float vertical;
    }

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct RotationData
    {
        [FieldOffset(0)]
        public float yaw;

        [FieldOffset(4)]
        public float pitch;

        [FieldOffset(8)]
        public float roll;
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct WheelData
    {
        [FieldOffset(0)]
        public float rearLeftWheel;

        [FieldOffset(4)]
        public float rearRightWheel;

        [FieldOffset(8)]
        public float frontLeftWheel;

        [FieldOffset(12)]
        public float frontRightWheel;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BrakeData
    {
        [FieldOffset(0)]
        public ushort rearLeftBrake;

        [FieldOffset(2)]
        public ushort rearRightBrake;

        [FieldOffset(4)]
        public ushort frontLeftBrake;

        [FieldOffset(6)]
        public ushort frontRightBrake;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct TyreTemperatureData
    {
        [FieldOffset(0)]
        public byte rearLeftTyre;

        [FieldOffset(1)]
        public byte rearRightTyre;

        [FieldOffset(2)]
        public byte frontLeftTyre;

        [FieldOffset(3)]
        public byte frontRightTyre;
    }

    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct TyrePressureData
    {
        [FieldOffset(0)]
        public float rearLeftTyre;

        [FieldOffset(1)]
        public float rearRightTyre;

        [FieldOffset(2)]
        public float frontLeftTyre;

        [FieldOffset(3)]
        public float frontRightTyre;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct SurfaceTypeData
    {
        [FieldOffset(0)]
        public byte rearLeftSurface;

        [FieldOffset(1)]
        public byte rearRightSurface;

        [FieldOffset(2)]
        public byte frontLeftSurface;

        [FieldOffset(3)]
        public byte frontRightSurface;

    }
}
