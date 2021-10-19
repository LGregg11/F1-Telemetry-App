namespace F1_Telemetry_App.Model
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using UdpPackets;

    public static class TelemetryReader
    {
        public static int TELEMETRY_HEADER_SIZE = 24;

        public static EventType GetEventType(byte[] remainingPacket)
        {
            try 
            {
                return (EventType)Enum.Parse(
                    typeof(EventType),
                    Encoding.ASCII.GetString(remainingPacket.Take(4).ToArray())
                    );
            }
            catch
            {
                // Return an unknown event type instead of an error
                return EventType.UNKNOWN;
            }
        }

        public static T ByteArrayToUdpPacketStruct<T>(byte[] remainingPacket)
        {
            GCHandle handle = GCHandle.Alloc(remainingPacket, GCHandleType.Pinned);
            T packet;
            try
            {
                packet = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return packet;
        }

        public static object GetEventStructType(byte[] remainingPacket)
        {
            EventType eventType = GetEventType(remainingPacket);
            switch (eventType)
            {
                case EventType.BUTN:
                    return ByteArrayToUdpPacketStruct<Buttons>(remainingPacket);
                case EventType.DTSV:
                    return ByteArrayToUdpPacketStruct<DriveThroughPenaltyServed>(remainingPacket);
                case EventType.FLBK:
                    return ByteArrayToUdpPacketStruct<Flashback>(remainingPacket);
                case EventType.FTLP:
                    return ByteArrayToUdpPacketStruct<FastestLap>(remainingPacket);
                case EventType.PENA:
                    return ByteArrayToUdpPacketStruct<Penalty>(remainingPacket);
                case EventType.RCWN:
                    return ByteArrayToUdpPacketStruct<RaceWinner>(remainingPacket);
                case EventType.RTMT:
                    return ByteArrayToUdpPacketStruct<Retirement>(remainingPacket);
                case EventType.SGSV:
                    return ByteArrayToUdpPacketStruct<StopGoPenaltyServed>(remainingPacket);
                case EventType.SPTP:
                    return ByteArrayToUdpPacketStruct<SpeedTrap>(remainingPacket);
                case EventType.STLG:
                    return ByteArrayToUdpPacketStruct<StartLights>(remainingPacket);
                case EventType.TMPT:
                    return ByteArrayToUdpPacketStruct<TeamMateInPits>(remainingPacket);
                case EventType.SSTA:
                case EventType.LGOT:
                case EventType.DRSE:
                case EventType.DRSD:
                case EventType.CHQF:
                case EventType.SEND:
                case EventType.UNKNOWN:
                default:
                    return null;
            }
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
    }
}
