namespace F1GameTelemetry.Packets.Enums
{
    using System.ComponentModel;

    #region  PacketIds
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
    #endregion

    #region EventType
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
    #endregion

    #region SurfaceTypes
    public enum SurfaceTypes : byte
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
    #endregion

    #region MFDPanelIndexTypes
    public enum MFDPanelIndexTypes : byte
    {
        Closed = 255,

        // Apparently this might depend on game mode? (Below is for 'single mode')
        CarSetup = 0,
        Pits = 1,
        Damage = 2,
        Engine = 3,
        Temperatures = 4
    }
    #endregion

    #region TyreCompoundTypes
    public enum TyreCompoundTypes : byte
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
    #endregion

    #region TyreVisualTypes
    public enum TyreVisualTypes : byte
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
    #endregion

    #region FiaFlagTypes
    public enum FiaFlagTypes : sbyte
    {
        Unknown = -1,
        None = 0,
        Greed = 1,
        Blue = 2,
        Yellow = 3,
        Red = 4
    }
    #endregion

    #region ErsDeploymentModes
    public enum ErsDeploymentModes : byte
    {
        None = 0,
        Medium = 1,
        Hotlap = 2,
        Overtake = 3
    }
    #endregion

    #region TractionControlTypes
    public enum TractionControlTypes : byte
    {
        Off = 0,
        Medium = 1,
        Full = 2
    }
    #endregion

    #region FuelMix
    public enum FuelMix : byte
    {
        Lean = 0,
        Standard = 1,
        Rich = 2,
        Max = 3
    }
    #endregion
}
