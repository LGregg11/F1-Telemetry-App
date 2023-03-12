namespace F1GameTelemetry.Readers.F12021;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Packets;
using F1GameTelemetry.Packets.F12021;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        Task headerTask = new(() => HeaderPacket?.ReceivePacket(e.Message));
        headerTask.RunSynchronously();

        byte[] remainingPacket = e.Message.Skip(HeaderPacketSize).ToArray();
        Task remainingTask = new(() => RaiseEventHandler((PacketId)header.packetId, remainingPacket));
        remainingTask.RunSynchronously();
    }

    public object? GetEvent(byte[] remainingPacket)
    {
        EventType eventType = GetEventType(remainingPacket);
        return eventType switch
        {
            EventType.BUTN => Converter.BytesToPacket<Buttons>(remainingPacket),
            EventType.FLBK => Converter.BytesToPacket<Flashback>(remainingPacket),
            EventType.STLG => Converter.BytesToPacket<StartLights>(remainingPacket),
            EventType.LGOT => Converter.BytesToPacket<StartLights>(remainingPacket),
            EventType.SPTP => Converter.BytesToPacket<SpeedTrap>(remainingPacket),
            EventType.FTLP => Converter.BytesToPacket<FastestLap>(remainingPacket),
            EventType.TMPT => Converter.BytesToPacket<TeamMateInPits>(remainingPacket),
            EventType.RCWN => Converter.BytesToPacket<RaceWinner>(remainingPacket),
            EventType.RTMT => Converter.BytesToPacket<Retirement>(remainingPacket),
            EventType.PENA => Converter.BytesToPacket<Penalty>(remainingPacket),
            EventType.SGSV => Converter.BytesToPacket<StopGoPenaltyServed>(remainingPacket),
            EventType.DTSV => Converter.BytesToPacket<DriveThroughPenaltyServed>(remainingPacket),
            _ => null,
        };
    }

    public override void RaiseEventHandler(PacketId id, byte[] remainingPacket)
    {
        if (_packetMap.ContainsKey(id))
            _packetMap[id].ReceivePacket(remainingPacket);
    }
}
