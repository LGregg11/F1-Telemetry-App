namespace F1GameTelemetry.Packets.F12022
{
    using Enums;

    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 924)]
    public struct CarDamage
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarDamageData[] carDamageData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 42)]
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
        public Fault drsFault;
        public Fault ersFault; // New to 2022
        public byte gearBoxDamage;
        public byte engineDamage;
        public byte engineMGUHWear;
        public byte engineESWear;
        public byte engineCEWear;
        public byte engineICEWear;
        public byte engineMGUKWear;
        public byte engineICWear;
        public Fault engineBlown; // New to 2022
        public Fault engineSeized; // New to 2022
    }
}
