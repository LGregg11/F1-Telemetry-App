namespace F1GameTelemetry.Enums
{
    using System.ComponentModel;

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
        SophieLevasseur = 25,
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

    public enum DriverStatus : byte
    {
        InGarage = 0,
        FlyingLap = 1,
        InLap = 2,
        OutLap = 3,
        OnTrack = 4
    }

    public enum AiControlled : byte
    {
        Human = 0,
        AI = 1
    }

    public enum PitStatus : byte
    {
        None = 0,
        Pitting = 1,
        InPitArea = 2
    }

    public enum MyTeam : byte
    {
        Other = 0,
        MyTeam = 1
    }

    public enum ReadyStatus : byte
    {
        NotReady = 0,
        Ready = 1,
        Spectating = 2
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

}
