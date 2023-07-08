namespace F1GameTelemetry.Packets.F12021;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1323)]
public struct CarTelemetry
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarTelemetryData[] carTelemetryData;

    public MFDPanelIndexType mfdPanelIndex;
    public MFDPanelIndexType mfdPanelIndexSecondaryPlayer;
    public sbyte suggestedGear; // 1-8 (0 if no gear suggested)
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 60)]
public struct CarTelemetryData
{
    public ushort speed; // km/h
    public float throttle; // 0.0 - 1.0
    public float steer; // -1.0 (full lock left) - 1.0 (full lock right)
    public float brake; // 0.0 - 1.0
    public byte clutch; // 0 - 100
    public sbyte gear; // Gears=1-8, Neutral=0, Reverse=-1
    public ushort engineRPM;
    public byte drs; // 0 = Off, 1 = On
    public byte revLightsPercent;
    public ushort revLightsBitValue; // bit 0 = leftmost LED, bit 14 = rightmost LED

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public ushort[] brakesTemperature;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] tyresSurfaceTemperature;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] tyresInnerTemperature;

    public ushort engineTemperature;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] tyrePressure;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public SurfaceType[] surfaceType;
}
