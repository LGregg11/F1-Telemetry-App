namespace F1GameTelemetry.Packets.Enums
{
    using System.ComponentModel;

    public enum PacketIds : byte
    {
        // Contains all motion data for player's car - only sent while player is in control
        Motion = 0,

        // Data about the session - track, time left
        Session = 1,

        // Data about all the lap times of cars in the session
        LapData = 2,

        // Various notable events that happen during a session
        Event = 3,

        // List of participants in the session, mostly relevant for multiplayer
        Participants = 4,

        // Packet detailing car setups for cars in the race
        CarSetups = 5,

        // Telemetry data for all cars
        CarTelemetry = 6,

        // Status data for all cars
        CarStatus = 7,

        // Final classification confirmation at the end of a race
        FinalClassification = 8,

        // Information about players in a multiplayer lobby
        LobbyInfo = 9,

        // Damage status for all cars
        CarDamage = 10,

        // Lap and tyre data for session
        SessionHistory = 11
    }

    public enum EventType
    {
        [Description("Unknown Event Type")]
        UNKNOWN,

        [Description("Session Started")]
        SSTA,

        [Description("Session Ended")]
        SEND,

        [Description("Fastest Lap")]
        FTLP,

        [Description("Retirement")]
        RTMT,

        [Description("DRS Enabled")]
        DRSE,

        [Description("DRS Disabled")]
        DRSD,

        [Description("Team mate in pits")]
        TMPT,

        [Description("Chequered flag")]
        CHQF,

        [Description("Race Winner")]
        RCWN,

        [Description("Penalty Issued")]
        PENA,

        [Description("Speed Trap Triggered")]
        SPTP,

        [Description("Start lights")]
        STLG,

        [Description("Lights out")]
        LGOT,

        [Description("Drive through served")]
        DTSV,

        [Description("Stop go served")]
        SGSV,

        [Description("Flashback")]
        FLBK,

        [Description("Button status")]
        BUTN
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

    public enum MFDPanelIndexType : byte
    {
        Closed = 255,

        // Apparently this might depend on game mode? (Below is for 'single mode')
        CarSetup = 0,
        Pits = 1,
        Damage = 2,
        Engine = 3,
        Temperatures = 4
    }

    public enum TyreCompoundType : byte
    {
        // F1 Modern
        C5 = 16,
        C4 = 17,
        C3 = 18,
        C2 = 19,
        C1 = 20,
        Intermediate = 7,
        Wet = 8,

        // F1 Classic
        ClassicDry = 9,
        ClassicWet = 10,

        // F2
        F2SuperSoft = 11,
        F2Soft = 12,
        F2Medium = 13,
        F2Hard = 14,
        F2Wet = 15
    }

    public enum TyreVisualType : byte
    {
        // F1 Modern & Classic
        Soft =  16,
        Medium = 17,
        Hard = 18,
        Intermediate = 7,
        Wet = 8,

        // F2 '19
        F2Wet = 15,
        F2SuperSoft = 19,
        F2Soft = 20,
        F2Medium = 21,
        F2Hard = 22

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

    public enum ErsDeploymentMode : byte
    {
        None = 0,
        Medium = 1,
        Hotlap = 2,
        Overtake = 3
    }

    public enum TractionControlType : byte
    {
        Off = 0,
        Medium = 1,
        Full = 2
    }

    public enum FuelMix : byte
    {
        Lean = 0,
        Standard = 1,
        Rich = 2,
        Max = 3
    }

    public enum ResultStatus : byte
    {
        Invalid = 0,
        Inactive = 1,
        Active = 2,
        Finished = 3,
        DNF = 4,
        DSQ = 5,
        NotClassified = 6,
        Retired = 7
    }

    public enum PitStatus : byte
    {
        None = 0,
        Pitting = 1,
        InPitArea = 2
    }

    public enum Sector : byte
    {
        Sector1 = 0,
        Sector2 = 1,
        Sector3 = 2
    }

    public enum DriverStatus : byte
    {
        InGarage = 0,
        FlyingLap = 1,
        InLap = 2,
        OutLap = 3,
        OnTrack = 4
    }

    public enum Weather : byte
    {
        Unknown = 255,

        Clear = 0,
        LightCloud = 1,
        Overcast = 2,
        LightRain = 3,
        HeavyRain = 4,
        Storm = 5
    }

    public enum SessionType : byte
    {
        Unknown = 0,
        P1 = 1,
        P2 = 2,
        P3 = 3,
        ShortP = 4,
        Q1 = 5,
        Q2 = 6,
        Q3 = 7,
        ShortQ = 8,
        OSQ = 9,
        R = 10,
        R2 = 11,
        R3 = 12,
        TimeTrial = 13
    }

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

    public enum Formula : byte
    {
        F1Modern = 0,
        F1Classic = 1,
        F2 = 2,
        F1Generic = 3
    }

    public enum SafetyCarStatus : byte
    {
        None = 0,
        Full = 1,
        Virtual = 2,
        FormationLap = 3
    }

    public enum BasicAssist : byte
    {
        Off = 0,
        On = 1
    }

    public enum BrakingAssist : byte
    {
        Off = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum GearboxAssist : byte
    {
        Manual = 1,
        ManualAndSuggested = 2,
        Auto = 3
    }

    public enum DynamicRacingLine : byte
    {
        Off = 0,
        CornersOnly = 1,
        Full = 2
    }
    
    public enum DynamicRacingLineType : byte
    {
        TwoD = 0,
        ThreeD = 1
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

    public enum TemperatureChange : sbyte
    {
        Up = 0,
        Down = 1,
        NoChange = 2
    }

    public enum AiControlled : byte
    {
        Human = 0,
        AI = 1
    }

    public enum Driver : byte
    {
        // TODO: Add descriptions to these names..
        Human = 255,

        [Description("Carlos Sainz")]
        CarlosSainz = 0,
        DaniilKvyat = 1,
        DanielRicciardo = 2,
        FernandoAlonso = 3,
        FelipeMassa = 4,
        // No 5
        KimiRaikkonen = 6,
        LewisHamilton = 7,
        // No 8
        MaxVerstappen = 9,
        NicoHulkenberg = 10,
        KevinMagnussen = 11,
        RomainGrosjean = 12,
        SebastianVettel = 13,
        SergioPerez = 14,
        ValtteriBottas = 15,
        // No 16
        EstebanOcon = 17,
        // No 18
        LanceStroll = 19,
        ArronBarnes = 20,
        MartinGiles = 21,
        AlexMurray = 22,
        LucasRoth = 23,
        IgorCorreia = 24,
        SophieLevasseur= 25,
        JonasSchiffer = 26,
        AlainForest = 27,
        JayLetourneau = 28,
        EstoSaari = 29,
        YasarAtiyeh = 30,
        CallistoCalabresi = 31,
        NaotaIzum = 32,
        HowardClarke = 33,
        WilheimKaufmann = 34,
        MarieLaursen = 35,
        FlavioNieves = 36,
        PeterBelousov = 37,
        KlimekMichalski = 38,
        SantiagoMoreno = 39,
        BenjaminCoppens = 40,
        NoahVisser = 41,
        GertWaldmuller = 42,
        JulianQuesada = 43,
        DanielJones = 44,
        ArtemMarkelov = 45,
        TadasukeMakino = 46,
        SeanGelael = 47,
        NickDeVries = 48,
        JackAitken = 49,
        GeorgeRussell = 50,
        MaximilianGunther = 51,
        NireiFukuzumi = 52,
        LucaGhiotto = 53,
        LandoNorris = 54,
        SergioSetteCamara = 55,
        LouisDeletraz = 56,
        AntonioFuoco = 57,
        CharlesLeclerc = 58,
        PierreGasly = 59,
        // No 60 or 61
        AlexanderAlbon = 62,
        NicholasLatifi = 63,
        DorianBoccolacci = 64,
        NikoKari = 65,
        RobertoMerhi = 66,
        ArjunMaini = 67,
        AlessioLorandi = 68,
        RubenMeijer = 69,
        RashidNair = 70,
        JackTremblay = 71,
        DevonButler = 72,
        LukasWeber = 73,
        AntonioGiovinazzi = 74,
        RobertKubica = 75,
        AlainProst = 76,
        AyrtonSenna = 77,
        NobuharuMatsushita = 78,
        NikitaMazepin = 79,
        GuanyaZhou = 80,
        MickSchumacher = 81,
        CallumIlott = 82,
        JuanManualCorrea = 83,
        JordanKing = 84,
        MahaveerRaghunathan = 85,
        TatianaCalderon = 86,
        AnthoineHubert = 87,
        GuilianoAlesi = 88,
        RalphBoschung = 89,
        MichaelSchumacher = 90,
        DanTicktum = 91,
        MarcusArmstrong = 92,
        ChristianLundgaard = 93,
        YukiTsunoda = 94,
        JehanDaruvala = 95,
        GulhermeSamaia = 96,
        PedroPiquet = 97,
        FelipeDrugovich = 98,
        RobertSchwartzman = 99,
        RoyNissany = 100,
        MarinoSato = 101,
        AidanJackson = 102,
        CasperAkkerman = 103,
        // No 104 - 108
        JensonButton = 109,
        DavidCoulthard = 110,
        NicoRosberg = 111

    }

    public enum Team : byte
    {
        // TODO: Add descriptions to all thesse..
        Unknown = 255,

        // F1 Latest
        Mercedes = 0,
        Ferrari = 1,
        RedBullRacing = 2,
        Williams = 3,
        AstonMartin = 4,
        Alpine = 5,
        AlphaTauri = 6,
        Haas = 7,
        McLaren = 8,
        AlfaRomeo = 9,

        // 2019
        ArtGP19 = 42,
        Campos19 = 43,
        Carlin19 = 44,
        SauberJuniorCharouz19 = 45,
        Dams19 = 46,
        UniVirtuosi19 = 47,
        MPMotorsport19 = 48,
        Prema19 = 49,
        Trident19 = 50,
        Arden19 = 51,

        // F2 2020
        ArtGP20 = 70,
        Campos20 = 71,
        Carlin20 = 72,
        Charouz20 = 73,
        Dams20 = 74,
        UniVirtuosi20 = 75,
        MPMotorsport20 = 76,
        Prema20 = 77,
        Trident20 = 78,
        BWT20 = 79,
        Hitech20 = 80,

        // F1 2020
        Mercedes2020 = 85,
        Ferrari2020 = 86,
        RedBull2020 = 87,
        Williams2020 = 88,
        RacingPoint2020 = 89,
        Renault2020 = 90,
        AlphaTauri2020 = 91,
        Haas2020 = 92,
        McLaren2020 = 93,
        AlfaRomeo2020 = 94
    }

    public enum MyTeam : byte
    {
        Other = 0,
        MyTeam = 1
    }

    public enum Nationality : byte
    {
        Unknown,
        American,
        Argentinean,
        Australian,
        Austrian,
        Azerbaijani,
        Bahraini,
        Belgian,
        Bolivian,
        Brazilian,
        British,
        Bulgarian,
        Cameroonian,
        Canadian,
        Chilean,
        Chinese,
        Colombian,
        CostaRican,
        Croatian,
        Cypriot,
        Czech,
        Danish,
        Dutch,
        Ecuadorian,
        English,
        Emirian,
        Estonian,
        Finnish,
        French,
        German,
        Ghanaian,
        Greek,
        Guatemalan,
        Honduran,
        HongKonger,
        Hungarian,
        Icelander,
        Indian,
        Indonesian,
        Irish,
        Israeli,
        Italian,
        Jamaican,
        Japanese,
        Jordanian,
        Kuwaiti,
        Latvian,
        Lebanese,
        Lithuanian,
        Luxembourger,
        Malaysian,
        Maltese,
        Mexian,
        Monegasque,
        NewZealander,
        Nicaraguan,
        NorthernIrish,
        Norwegian,
        Omani,
        Pakistani,
        Panamanian,
        Paraguayan,
        Peruvian,
        Polish,
        Portuguese,
        Qatari,
        Romanian,
        Russian,
        Salvadoran,
        Saudi,
        Scottish,
        Serbian,
        Singaporean,
        Slovakian,
        Slovenian,
        SouthKorean,
        SouthAfrican,
        Spanish,
        Swedish,
        Swiss,
        Thai,
        Turkish,
        Uruguayan,
        Ukrainian,
        Venezuelan,
        Barbadian,
        Welsh,
        Vietnamese
    }

    public enum UdpSetting : byte
    {
        Restricted = 0,
        Public = 1
    }

    public enum ReadyStatus : byte
    {
        NotReady = 0,
        Ready = 1,
        Spectating = 2
    }

    public enum DrsFault : byte
    {
        OK = 0,
        Fault = 1
    }
}
