namespace F1GameTelemetry.Packets.F12023;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1320)]
public struct Motion
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarMotionData[] carMotionData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 60)]
public struct CarMotionData
{
    // Spec says to divide normalised vectors by 32767.0f to convert to floats
    // (assumes that direction values are always between -1.0f and 1.0f)

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] worldPosition; // x, y, z - metres

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] worldVelocity; // metres/s

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public short[] worldForwardDir; // Normalised

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public short[] worldRightDir; // Normalised

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] gForce; // lat, lon, vert

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] rotation; // roll, pitch, yaw
}
