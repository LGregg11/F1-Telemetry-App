namespace F1GameTelemetry.Packets.F12023
{
    using Enums;

    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1277)]
    public struct Participant
    {
        public byte numActiveCars;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public ParticipantData[] participants;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 58)]
    public struct ParticipantData
    {
        // "The array should be indexed by vehicle index"

        public AiControlled aiControlled;
        public DriverId driverId; // Driver IDs (255 for Human)
        public byte networkId;
        public Team teamId;
        public MyTeam myTeam;
        public byte raceNumber;
        public Nationality nationality;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] name; // UTF-8 format

        public UdpSetting yourTelemetry;
        public byte showOnlineNames; // New to 2023
        public Platform platform; // New to 2023
    }
}
