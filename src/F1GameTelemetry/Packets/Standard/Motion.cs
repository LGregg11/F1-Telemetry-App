namespace F1GameTelemetry.Packets.Standard;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1436)]
public struct Motion
{
    public Motion(CarMotionData[] carMotionData, ExtraCarMotionData extraCarMotionData)
    {
        this.carMotionData = carMotionData;
        this.extraCarMotionData = extraCarMotionData;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarMotionData[] carMotionData;

    public ExtraCarMotionData extraCarMotionData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 60)]
public struct CarMotionData
{
    public CarMotionData(
        float[] worldPosition,
        float[] worldVelocity,
        short[] worldForwardDir,
        short[] worldRightDir,
        float[] gForce,
        float[] rotation)
    {
        this.worldPosition = worldPosition;
        this.worldVelocity = worldVelocity;
        this.worldForwardDir = worldForwardDir;
        this.worldRightDir = worldRightDir;
        this.gForce = gForce;
        this.rotation = rotation;
    }

    // Spec says to divide normalised vectors by 32767.0f to convert to floats
    // (assumes that direction values are always between -1.0f and 1.0f)

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] worldPosition; // x, y, z

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] worldVelocity;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public short[] worldForwardDir; // Normalised

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public short[] worldRightDir; // Normalised

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] gForce; // lat, lon, vert

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] rotation; // roll, pitch, yaw
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 116)]
public struct ExtraCarMotionData
{
    public ExtraCarMotionData(
        float[] suspensionPosition,
        float[] suspensionVelocity,
        float[] suspensionAcceleration,
        float[] wheelSpeed,
        float[] wheelSlip,
        float[] localVelocity,
        float[] angularVelocity,
        float[] angularAcceleration)
    {
        this.suspensionPosition = suspensionPosition;
        this.suspensionVelocity = suspensionVelocity;
        this.suspensionAcceleration = suspensionAcceleration;
        this.wheelSpeed = wheelSpeed;
        this.wheelSlip = wheelSlip;
        this.localVelocity = localVelocity;
        this.angularVelocity = angularVelocity;
        this.angularAcceleration = angularAcceleration;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] suspensionPosition; // rear left, rear right, front left, front right

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] suspensionVelocity;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] suspensionAcceleration;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] wheelSpeed;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] wheelSlip;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] localVelocity; // x, y, z

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] angularVelocity;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] angularAcceleration;
}
