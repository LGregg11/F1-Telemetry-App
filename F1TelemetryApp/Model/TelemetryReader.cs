namespace F1_Telemetry_App.Model
{
    using System.Runtime.InteropServices;
    using UdpPackets;

    public static class TelemetryReader
    {
        public static int TELEMETRY_HEADER_SIZE = 24;
        public static UdpPacketHeader ByteArrayToUdpPacketHeader(byte[] packet)
        {
            GCHandle handle = GCHandle.Alloc(packet, GCHandleType.Pinned);
            UdpPacketHeader header;
            try
            {
                header = (UdpPacketHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(UdpPacketHeader));
            }
            finally
            {
                handle.Free();
            }
            return header;
        }

        #region Enums
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
    }
}
