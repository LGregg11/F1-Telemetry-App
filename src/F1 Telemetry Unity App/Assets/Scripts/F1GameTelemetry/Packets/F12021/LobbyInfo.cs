namespace F1GameTelemetry.Packets.F12021
{
    using Enums;

    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1167)]
    public struct LobbyInfo
    {
        public byte numPlayers;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public LobbyInfoData[] lobbyPlayers;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 53)]
    public struct LobbyInfoData
    {
        public AiControlled aiControlled;
        public Team teamId;
        public Nationality nationality;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] name; // Will be truncated with '...' (U+2026) if too long

        public byte carNumber;
        public ReadyStatus readyStatus;
    }
}
