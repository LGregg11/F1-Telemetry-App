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
        public static int TELEMETRY_HEADER_SIZE = 24;
        public static int CARMOTIONDATA_SIZE = 60;
        public static int CARTELEMETRYDATA_SIZE = 60;
        public static int LAPDATA_SIZE = 43;

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
            return ByteArrayToUdpPacketStruct<Motion>(remainingPacket);
        }

        public static CarTelemetry GetCarTelemetryStruct(byte[] remainingPacket)
        {
            return ByteArrayToUdpPacketStruct<CarTelemetry>(remainingPacket);
        }

        public static CarStatus GetCarStatusStruct(byte[] remainingPacket)
        {
            return ByteArrayToUdpPacketStruct<CarStatus>(remainingPacket);
        }

        public static FinalClassification GetFinalClassificationStruct(byte[] remainingPacket)
        {
            return ByteArrayToUdpPacketStruct<FinalClassification>(remainingPacket);
        }

        public static LapData GetLapDataStruct(byte[] remainingPacket)
        {
            return ByteArrayToUdpPacketStruct<LapData>(remainingPacket);
        }

        public static Session GetSessionStruct(byte[] remainingPacket)
        {
            return ByteArrayToUdpPacketStruct<Session>(remainingPacket);
        }

        public static Participant GetParticipantStruct(byte[] remainingPacket)
        {
            return ByteArrayToUdpPacketStruct<Participant>(remainingPacket);
        }
    }
}
