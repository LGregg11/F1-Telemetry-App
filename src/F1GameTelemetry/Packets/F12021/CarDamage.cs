namespace F1GameTelemetry.Packets.F12021;

using F1GameTelemetry.Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 858)]
public struct CarDamage
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarDamageData[] carDamageData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 39)]
public struct CarDamageData
{
    // All non-enum properties are percentage unless specified otherwise

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] tyreWear;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] tyreDamage;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] brakeDamage;

    public byte frontLeftWingDamage;
    public byte frontRightWingDamage;
    public byte rearWingDamage;
    public byte floorDamage;
    public byte diffuserDamage;
    public byte sidepodDamage;
    public DrsFault drsFault;
    public byte gearBoxDamage;
    public byte engineDamage;
    public byte engineMGUHWear;
    public byte engineESWear;
    public byte engineCEWear;
    public byte engineICEWear;
    public byte engineMGUKWear;
    public byte engineICWear;
}
