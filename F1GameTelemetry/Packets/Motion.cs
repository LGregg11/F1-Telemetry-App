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
}
