namespace F1GameTelemetry.Packets.Standard;

using F1GameTelemetry.Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 858)]
public struct CarDamage
{
    public CarDamage(CarDamageData[] carDamageData)
    {
        this.carDamageData = carDamageData;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarDamageData[] carDamageData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 39)]
public struct CarDamageData
{
    public CarDamageData(
        float[] tyreWear,
        byte[] tyreDamage,
        byte[] brakeDamage,
        byte frontLeftWingDamage,
        byte frontRightWingDamage,
        byte rearWingDamage,
        byte floorDamage,
        byte diffuserDamage,
        byte sidepodDamage,
        DrsFault drsFault,
        byte gearBoxDamage,
        byte engineDamage,
        byte engineMGUHWear,
        byte engineESWear,
        byte engineCEWear,
        byte engineICEWear,
        byte engineMGUKWear,
        byte engineICWear)
    {
        this.tyreWear = tyreWear;
        this.tyreDamage = tyreDamage;
        this.brakeDamage = brakeDamage;
        this.frontLeftWingDamage = frontLeftWingDamage;
        this.frontRightWingDamage = frontRightWingDamage;
        this.rearWingDamage = rearWingDamage;
        this.floorDamage = floorDamage;
        this.diffuserDamage = diffuserDamage;
        this.sidepodDamage = sidepodDamage;
        this.drsFault = drsFault;
        this.gearBoxDamage = gearBoxDamage;
        this.engineDamage = engineDamage;
        this.engineMGUHWear = engineMGUHWear;
        this.engineESWear = engineESWear;
        this.engineCEWear = engineCEWear;
        this.engineICEWear = engineICEWear;
        this.engineMGUKWear = engineMGUKWear;
        this.engineICWear = engineICWear;
    }

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
