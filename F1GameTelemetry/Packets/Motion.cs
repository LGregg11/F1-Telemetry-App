namespace F1GameTelemetry.Packets
{
    using System.Runtime.InteropServices;

    public struct Motion
    {
        public CarMotionData[] carMotionData;

        public ExtraCarMotionData extraCarMotionData;
    }

    [StructLayout(LayoutKind.Explicit, Size = 60)]
    public struct CarMotionData
    {
        // Spec says to divide normalised vectors by 32767.0f to convert to floats
        // (assumes that direction values are always between -1.0f and 1.0f)

        [FieldOffset(0)]
        public VectorData worldPosition;

        [FieldOffset(12)]
        public VectorData worldVelocity;

        [FieldOffset(24)]
        public NormalisedVectorData worldForwardDir; // Normalised

        [FieldOffset(30)]
        public NormalisedVectorData worldRightDir; // Normalised

        [FieldOffset(36)]
        public CoordinateData gForce;

        [FieldOffset(48)]
        public RotationData rotation;
    }

    [StructLayout(LayoutKind.Explicit, Size = 116)]
    public struct ExtraCarMotionData
    {
        [FieldOffset(0)]
        public WheelData suspensionPosition;

        [FieldOffset(16)]
        public WheelData suspensionVelocity;

        [FieldOffset(32)]
        public WheelData suspensionAcceleration;

        [FieldOffset(48)]
        public WheelData wheelSpeed;

        [FieldOffset(64)]
        public WheelData wheelSlip;

        [FieldOffset(80)]
        public VectorData localVelocity;

        [FieldOffset(92)]
        public VectorData angularVelocity;

        [FieldOffset(104)]
        public VectorData angularAcceleration;
    }

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
        public float rearLeft;

        [FieldOffset(4)]
        public float rearRight;

        [FieldOffset(8)]
        public float frontLeft;

        [FieldOffset(12)]
        public float frontRight;
    }
}
