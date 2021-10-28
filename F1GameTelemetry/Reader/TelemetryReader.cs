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

        public static MFDPanelIndexTypes GetMfdPanelIndexType(byte[] remainingPacket)
        {
            try
            {
                return (MFDPanelIndexTypes)remainingPacket.FirstOrDefault();
            }
            catch
            {
                // Return 'closed' instead of an error
                return MFDPanelIndexTypes.Closed;
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
            CarMotionData[] carMotionData = new CarMotionData[MAX_CARS_PER_RACE];

            for (int i=0; i<MAX_CARS_PER_RACE; i++)
            {
                CarMotionData carData = ByteArrayToUdpPacketStruct<CarMotionData>(
                    remainingPacket.Skip(i*CARMOTIONDATA_SIZE).ToArray());
                carMotionData[i] = carData;
            }
            ExtraCarMotionData extraCarMotionData = ByteArrayToUdpPacketStruct<ExtraCarMotionData>(
                remainingPacket.Skip(MAX_CARS_PER_RACE*CARMOTIONDATA_SIZE).ToArray());

            return new Motion
            {
                carMotionData = carMotionData,
                extraCarMotionData = extraCarMotionData
            };
        }

        public static CarTelemetry GetCarTelemetryStruct(byte[] remainingPacket)
        {
            CarTelemetryData[] carTelemetryData = new CarTelemetryData[MAX_CARS_PER_RACE];
            for (int i=0; i<MAX_CARS_PER_RACE; i++)
            {
                CarTelemetryData telemetryData = ByteArrayToUdpPacketStruct<CarTelemetryData>(
                    remainingPacket.Skip(i * CARTELEMETRYDATA_SIZE).ToArray());

                carTelemetryData[i] = telemetryData;
            }
            remainingPacket = remainingPacket.Skip(MAX_CARS_PER_RACE * CARTELEMETRYDATA_SIZE).ToArray();
            MFDPanelIndexTypes mfdPanelIndex = GetMfdPanelIndexType(remainingPacket);
            remainingPacket = remainingPacket.Skip(1).ToArray();
            MFDPanelIndexTypes mfdPanelIndexSecondaryType = GetMfdPanelIndexType(remainingPacket);
            int suggestedGear = Convert.ToInt32(remainingPacket.Skip(1).ToArray().FirstOrDefault());

            return new CarTelemetry
            {
                carTelemetryData = carTelemetryData,
                mfdPanelIndex = mfdPanelIndex,
                mfdPanelIndexSecondaryPlayer = mfdPanelIndexSecondaryType,
                suggestedGear = suggestedGear
            };
        }
    }
}
