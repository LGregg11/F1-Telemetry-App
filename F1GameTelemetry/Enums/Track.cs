namespace F1GameTelemetry.Enums
{
    public enum Track : sbyte
    {
        Unknown = -1,

        Melbourne = 0,
        PaulRicard = 1,
        Shanghai = 2,
        Sakhir = 3,
        Catalunya = 4,
        Moncao = 5,
        Montreal = 6,
        Silverstone = 7,
        Hockenheim = 8,
        Hungaroring = 9,
        Spa = 10,
        Monza = 11,
        Singapore = 12,
        Suzuka = 13,
        AbuDhabi = 14,
        Texas = 15,
        Brazil = 16,
        Austria = 17,
        Sochi = 18,
        Mexico = 19,
        Baku = 20,
        SakhirShort = 21,
        SilverstoneShort = 22,
        TexasShort = 23,
        SuzukaShort = 24,
        Hanoi = 25,
        Zandvoort = 26,
        Imola = 27,
        Portimao = 28,
        Jeddah = 29
    }

    public enum SurfaceType : byte
    {
        Tarmac = 0,
        RumbleStrip = 1,
        Concrete = 2,
        Rock = 3,
        Gravel = 4,
        Mud = 5,
        Sand = 6,
        Grass = 7,
        Water = 8,
        Cobblestone = 9,
        Metal = 10,
        Ridged = 11
    }

    public enum Sector : byte
    {
        Sector1 = 0,
        Sector2 = 1,
        Sector3 = 2
    }

    public enum FiaFlagType : sbyte
    {
        Unknown = -1,
        None = 0,
        Greed = 1,
        Blue = 2,
        Yellow = 3,
        Red = 4
    }

    public enum ZoneFlag : sbyte
    {
        Unknown = -1,
        None = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        Red = 4
    }
}
