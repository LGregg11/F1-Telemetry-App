namespace F1GameTelemetry.Reader
{
    using F1GameTelemetry.Packets;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class TelemetryReader
    {
        public static int TELEMETRY_HEADER_SIZE = 24;
        public static int TELEMETRY_CARMOTIONDATA_SIZE = 60;
        public static int MAX_CARS_PER_RACE = 22;

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

        public static object GetEventStruct(byte[] remainingPacket)
        {
            EventType eventType = GetEventType(remainingPacket);
            switch (eventType)
            {
                case EventType.BUTN:
                    return ByteArrayToUdpPacketStruct<Buttons>(remainingPacket);
                case EventType.FLBK:
                    return ByteArrayToUdpPacketStruct<Flashback>(remainingPacket);
                case EventType.STLG:
                    return ByteArrayToUdpPacketStruct<StartLights>(remainingPacket);
                case EventType.LGOT:
                    return ByteArrayToUdpPacketStruct<StartLights>(remainingPacket);
                case EventType.SPTP:
                    return ByteArrayToUdpPacketStruct<SpeedTrap>(remainingPacket);
                case EventType.FTLP:
                    return ByteArrayToUdpPacketStruct<FastestLap>(remainingPacket);
                case EventType.TMPT:
                    return ByteArrayToUdpPacketStruct<TeamMateInPits>(remainingPacket);
                case EventType.RCWN:
                    return ByteArrayToUdpPacketStruct<RaceWinner>(remainingPacket);
                case EventType.RTMT:
                    return ByteArrayToUdpPacketStruct<Retirement>(remainingPacket);
                case EventType.PENA:
                    return ByteArrayToUdpPacketStruct<Penalty>(remainingPacket);
                case EventType.SGSV:
                    return ByteArrayToUdpPacketStruct<StopGoPenaltyServed>(remainingPacket);
                case EventType.DTSV:
                    return ByteArrayToUdpPacketStruct<DriveThroughPenaltyServed>(remainingPacket);
                case EventType.SSTA:
                // Session Started
                case EventType.SEND:
                // Session End
                case EventType.DRSE:
                // DRS Enabled
                case EventType.DRSD:
                // DRS Disabled
                case EventType.CHQF:
                // Chequered Flag
                case EventType.UNKNOWN:
                default:
                    return null;
            }
        }

        public static Motion GetMotionStruct(byte[] remainingPacket)
        {
            CarMotionData[] carMotionData = new CarMotionData[22];

            for (int i=1; i<=MAX_CARS_PER_RACE; i++)
            {

                CarMotionData carData = ByteArrayToUdpPacketStruct<CarMotionData>(remainingPacket.Skip(i*TELEMETRY_CARMOTIONDATA_SIZE).ToArray());
                carMotionData[i-1] = carData;
            }
            ExtraCarMotionData extraCarMotionData = ByteArrayToUdpPacketStruct<ExtraCarMotionData>(
                remainingPacket.Skip(MAX_CARS_PER_RACE*TELEMETRY_CARMOTIONDATA_SIZE).ToArray());

            return new Motion
            {
                carMotionData = carMotionData,
                extraCarMotionData = extraCarMotionData
            };
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
