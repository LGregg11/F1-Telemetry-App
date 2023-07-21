namespace F1GameTelemetry.Packets.F12023;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 188)]
public struct ExtraCarMotionData
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] suspensionPosition; // rear left, rear right, front left, front right

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] suspensionVelocity;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] suspensionAcceleration;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] wheelSpeed;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] wheelSlipRatio;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] wheelSlipAngle;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] wheelLatForce;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] wheelLongForce;

    public float heightOfCOGAboveGround;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] localVelocity; // x, y, z - metres/s

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] angularVelocity; // radians/s

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] angularAcceleration; // radians/s/s

    public float frontWheelsAngle; // radians

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] wheelVertForce;
}
