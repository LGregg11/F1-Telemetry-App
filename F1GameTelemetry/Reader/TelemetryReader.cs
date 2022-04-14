namespace F1GameTelemetry.Reader
{
    using F1GameTelemetry.Packets;
    using F1GameTelemetry.Packets.Enums;
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class TelemetryReader
    {
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

        public static MFDPanelIndexType GetMfdPanelIndexType(byte[] remainingPacket)
        {
            try
            {
                return (MFDPanelIndexType)remainingPacket.FirstOrDefault();
            }
            catch
            {
                // Return 'closed' instead of an error
                return MFDPanelIndexType.Closed;
            }
        }

        public static T BytesToPacket<T>(byte[] remainingPacket)
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

        public static object GetEvent(byte[] remainingPacket)
        {
            EventType eventType = GetEventType(remainingPacket);
            switch (eventType)
            {
                case EventType.BUTN:
                    return BytesToPacket<Buttons>(remainingPacket);
                case EventType.FLBK:
                    return BytesToPacket<Flashback>(remainingPacket);
                case EventType.STLG:
                    return BytesToPacket<StartLights>(remainingPacket);
                case EventType.LGOT:
                    return BytesToPacket<StartLights>(remainingPacket);
                case EventType.SPTP:
                    return BytesToPacket<SpeedTrap>(remainingPacket);
                case EventType.FTLP:
                    return BytesToPacket<FastestLap>(remainingPacket);
                case EventType.TMPT:
                    return BytesToPacket<TeamMateInPits>(remainingPacket);
                case EventType.RCWN:
                    return BytesToPacket<RaceWinner>(remainingPacket);
                case EventType.RTMT:
                    return BytesToPacket<Retirement>(remainingPacket);
                case EventType.PENA:
                    return BytesToPacket<Penalty>(remainingPacket);
                case EventType.SGSV:
                    return BytesToPacket<StopGoPenaltyServed>(remainingPacket);
                case EventType.DTSV:
                    return BytesToPacket<DriveThroughPenaltyServed>(remainingPacket);
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

        public static Motion GetMotion(byte[] remainingPacket) => BytesToPacket<Motion>(remainingPacket);

        public static CarTelemetry GetCarTelemetry(byte[] remainingPacket) => BytesToPacket<CarTelemetry>(remainingPacket);

        public static CarStatus GetCarStatus(byte[] remainingPacket) => BytesToPacket<CarStatus>(remainingPacket);

        public static FinalClassification GetFinalClassification(byte[] remainingPacket) => BytesToPacket<FinalClassification>(remainingPacket);

        public static LapData GetLapData(byte[] remainingPacket) => BytesToPacket<LapData>(remainingPacket);

        public static Session GetSession(byte[] remainingPacket) => BytesToPacket<Session>(remainingPacket);

        public static Participant GetParticipant(byte[] remainingPacket) => BytesToPacket<Participant>(remainingPacket);

        public static SessionHistory GetSessionHistory(byte[] remainingPacket) => BytesToPacket<SessionHistory>(remainingPacket);

        public static LobbyInfo GetLobbyInfo(byte[] remainingPacket) => BytesToPacket<LobbyInfo>(remainingPacket);

        public static CarDamage GetCarDamage(byte[] remainingPacket) => BytesToPacket<CarDamage>(remainingPacket);

        public static CarSetup GetCarSetup(byte[] remainingPacket) => BytesToPacket<CarSetup>(remainingPacket);
    }
}
