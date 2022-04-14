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
}
