namespace F1GameTelemetry.Readers.F12021
{
    using F1GameTelemetry.Packets.F12021;
    using F1GameTelemetry.Enums;
    using F1GameTelemetry.Listener;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using F1GameTelemetry.Packets;
    using F1GameTelemetry.Converters;

    public class TelemetryReader2021 : BaseTelemetryReader
    {
        private readonly Dictionary<PacketId, IPacket> _packetMap;
        // private readonly Dictionary<PacketId, IEvent> _eventMap;

        public TelemetryReader2021(ITelemetryListener listener) : base(listener)
        {
            HeaderPacket = new HeaderPacket();
            MotionPacket = new MotionPacket();
            SessionPacket = new SessionPacket();
            LapDataPacket = new LapDataPacket();
            ParticipantPacket = new ParticipantPacket();
            CarSetupPacket = new CarSetupPacket();
            CarTelemetryPacket = new CarTelemetryPacket();
            CarStatusPacket = new CarStatusPacket();
            FinalClassificationPacket = new FinalClassificationPacket();
            LobbyInfoPacket = new LobbyInfoPacket();
            CarDamagePacket = new CarDamagePacket();
            SessionHistoryPacket = new SessionHistoryPacket();

            _packetMap = new Dictionary<PacketId, IPacket>
            {
                { PacketId.Motion, MotionPacket},
                { PacketId.Session, SessionPacket },
                { PacketId.LapData, LapDataPacket },
                // { PacketId.Event, new EventPacket() },
                { PacketId.Participant, ParticipantPacket },
                { PacketId.CarSetup, CarSetupPacket },
                { PacketId.CarTelemetry, CarTelemetryPacket },
                { PacketId.CarStatus, CarStatusPacket },
                { PacketId.FinalClassification, FinalClassificationPacket },
                { PacketId.LobbyInfo, LobbyInfoPacket },
                { PacketId.CarDamage, CarDamagePacket },
                { PacketId.SessionHistory, SessionHistoryPacket }
            };
        }

        public override string Name => "F1 2021 Telemetry Reader";

        public override bool IsSupported => true;

        public override IPacket HeaderPacket { get; }

        public override IPacket MotionPacket { get; }

        public override IPacket CarTelemetryPacket { get; }

        public override IPacket CarStatusPacket { get; }

        public override IPacket FinalClassificationPacket { get; }

        public override IPacket LapDataPacket { get; }

        public override IPacket SessionPacket { get; }

        public override IPacket ParticipantPacket { get; }

        public override IPacket SessionHistoryPacket { get; }

        public override IPacket LobbyInfoPacket { get; }

        public override IPacket CarDamagePacket { get; }

        public override IPacket CarSetupPacket { get; }

        public override void OnTelemetryReceived(object sender, TelemetryEventArgs e)
        {
            Header header = Converter.BytesToPacket<Header>(e.Message);
            Task headerTask = new Task(() => HeaderPacket?.ReceivePacket(e.Message));
            headerTask.RunSynchronously();

            byte[] remainingPacket = e.Message.Skip(HeaderPacketSize).ToArray();
            Task remainingTask = new Task(() => RaiseEventHandler((PacketId)header.packetId, remainingPacket));
            remainingTask.RunSynchronously();
        }

        public object? GetEvent(byte[] remainingPacket)
        {
            EventType eventType = GetEventType(remainingPacket);
            switch (eventType)
            {
                case EventType.BUTN:
                    return Converter.BytesToPacket<Buttons>(remainingPacket);
                case EventType.FLBK:
                    return Converter.BytesToPacket<Flashback>(remainingPacket);
                case EventType.STLG:
                    return Converter.BytesToPacket<StartLights>(remainingPacket);
                case EventType.LGOT:
                    return Converter.BytesToPacket<StartLights>(remainingPacket);
                case EventType.SPTP:
                    return Converter.BytesToPacket<SpeedTrap>(remainingPacket);
                case EventType.FTLP:
                    return Converter.BytesToPacket<FastestLap>(remainingPacket);
                case EventType.TMPT:
                    return Converter.BytesToPacket<TeamMateInPits>(remainingPacket);
                case EventType.RCWN:
                    return Converter.BytesToPacket<RaceWinner>(remainingPacket);
                case EventType.RTMT:
                    return Converter.BytesToPacket<Retirement>(remainingPacket);
                case EventType.PENA:
                    return Converter.BytesToPacket<Penalty>(remainingPacket);
                case EventType.SGSV:
                    return Converter.BytesToPacket<StopGoPenaltyServed>(remainingPacket);
                case EventType.DTSV:
                    return Converter.BytesToPacket<DriveThroughPenaltyServed>(remainingPacket);
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

        public override void RaiseEventHandler(PacketId id, byte[] remainingPacket)
        {
            if (_packetMap.ContainsKey(id))
                _packetMap[id].ReceivePacket(remainingPacket);
        }
    }
}
