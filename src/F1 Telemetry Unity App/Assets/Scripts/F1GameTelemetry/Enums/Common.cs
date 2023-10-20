namespace F1GameTelemetry.Enums
{
    using System.ComponentModel;

    public enum PacketId : byte
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
        Participant = 4,

        // Packet detailing car setups for cars in the race
        CarSetup = 5,

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
        SessionHistory = 11,

        // New for 2023 - Extended tyre set data
        TyreSets = 12,

        // New for 2023 - Extended motion data for player car
        MotionEx = 13
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

    public enum Team : byte
    {
        // TODO: Add descriptions to all these..
        Unknown = 255,

        // F1 2021
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
        AlfaRomeo2020 = 94,

        // F1 2022 - Supercars
        AstonMartinDB11V12 = 95,
        AstonMartinVantageF1Edition = 96,
        AstonMartinVantageSafetyCar = 97,
        FerrariF8Tributo = 98,
        FerrariRoma = 99,
        McLaren720S = 100,
        McLarenArtura = 101,
        MercedesAMGGTBlackSeriesSafetyCar = 102,
        MercedesAMGGTRPro = 103,

        // F1 2022
        F1CustomTeam = 104,

        Prema21 = 106,
        UniVirtuosi21 = 107,
        Carlin21 = 108,
        Hitech21 = 109,
        ArtGP21 = 110,
        MPMotorsport21 = 111,
        Charouz21 = 112,
        Dams21 = 113,
        Campos21 = 114,
        BWT21 = 115,
        Trident21 = 116,
        MercedesAMGGTBlackSeries = 117,

        Prema22 = 118,
        Virtuosi22 = 119,
        Carlin22 = 120,
        Hitech22 = 121,
        ArtGP22 = 122,
        MPMotorsport22 = 123,
        Charouz22 = 124,
        Dams22 = 125,
        Campos22 = 126,
        VanAmersfoortRacing22 = 127,
        Trident22 = 128
    }

    public enum GameVersion : ushort
    {
        [Description("F1 2019")]
        F12019 = 2019,
        [Description("F1 2020")]
        F12020 = 2020,
        [Description("F1 2021")]
        F12021 = 2021,
        [Description("F1 2022")]
        F12022 = 2022,
        [Description("F1 2023")]
        F12023 = 2023
    }

    public enum Platform : byte
    {
        Unknown = 255,

        Steam = 1,
        Playstation = 3,
        Xbox = 4,
        Origin = 6
    }
}
