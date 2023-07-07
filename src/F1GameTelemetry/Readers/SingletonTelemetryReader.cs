namespace F1GameTelemetry.Readers;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Converters.F12021;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Events;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Packets.Standard;

using System.Collections.Generic;
using System.Threading.Tasks;

public static class SingletonTelemetryReader
{
    private static ITelemetryListener? _telemetryListener;
    private static ITelemetryConverter? _telemetryConverter;
    private static readonly List<ITelemetryConverter> _converters = new()
    {
        new TelemetryConverter2021()
    };

    public static event PacketEventHandler<Header>? HeaderReceived;
    public static event PacketEventHandler<Motion>? MotionReceived;
    public static event PacketEventHandler<Session>? SessionReceived;
    public static event PacketEventHandler<LapData>? LapDataReceived;
    public static event PacketEventHandler<Participant>? ParticipantReceived;
    public static event PacketEventHandler<CarSetup>? CarSetupReceived;
    public static event PacketEventHandler<CarTelemetry>? CarTelemetryReceived;
    public static event PacketEventHandler<CarStatus>? CarStatusReceived;
    public static event PacketEventHandler<FinalClassification>? FinalClassificationReceived;
    public static event PacketEventHandler<LobbyInfo>? LobbyInfoReceived;
    public static event PacketEventHandler<CarDamage>? CarDamageReceived;
    public static event PacketEventHandler<SessionHistory>? SessionHistoryReceived;

    public static void SetTelemetryListener(ITelemetryListener listener)
    {
        if (_telemetryListener != null)
            _telemetryListener.TelemetryReceived -= OnTelemetryReceived;

        _telemetryListener = listener;
        _telemetryListener.TelemetryReceived += OnTelemetryReceived;
    }

    public static bool IsListenerRunning => _telemetryListener != null && _telemetryListener.IsListenerRunning;

    public static void StartListener()
    {
        _telemetryListener?.Start();
    }

    public static void StopListener()
    {
        _telemetryListener?.Stop();
    }

    public static void SetTelemetryConverterByVersion(GameVersion version)
    {
        if (_telemetryConverter?.GameVersion == version)
            return;

        foreach (var converter in _converters)
        {
            if (converter.GameVersion == version)
            {
                _telemetryConverter = converter;
                return;
            }
        }
    }

    public static bool IsConverterSupported() => _telemetryConverter != null && _telemetryConverter.IsSupported;

    private static void OnTelemetryReceived(object sender, TelemetryEventArgs e)
    {
        Header header = Converter.BytesToPacket<Header>(e.Message);

        Task headerTask = new(() => HeaderReceived?.Invoke(typeof(SingletonTelemetryReader), new PacketEventArgs<Header>(header)));
        headerTask.RunSynchronously();

        Task remainingTask = new(() => ConvertAndRaiseEvent(header.packetId, e.Message));
        remainingTask.RunSynchronously();
    }
    private static void ConvertAndRaiseEvent(PacketId id, byte[] packet)
    {
        object? convertedPacket = _telemetryConverter?.ConvertBytesToStandardPacket(id, packet);

        switch (id)
        {
            case PacketId.Motion:
                MotionReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<Motion>((Motion)convertedPacket!));
                break;
            case PacketId.Session:
                SessionReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<Session>((Session)convertedPacket!));
                break;
            case PacketId.LapData:
                LapDataReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<LapData>((LapData)convertedPacket!));
                break;
            case PacketId.Event:
                // TODO: Events
                break;
            case PacketId.Participant:
                ParticipantReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<Participant>((Participant)convertedPacket!));
                break;
            case PacketId.CarSetup:
                CarSetupReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<CarSetup>((CarSetup)convertedPacket!));
                break;
            case PacketId.CarTelemetry:
                CarTelemetryReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<CarTelemetry>((CarTelemetry)convertedPacket!));
                break;
            case PacketId.CarStatus:
                CarStatusReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<CarStatus>((CarStatus)convertedPacket!));
                break;
            case PacketId.FinalClassification:
                FinalClassificationReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<FinalClassification>((FinalClassification)convertedPacket!));
                break;
            case PacketId.LobbyInfo:
                LobbyInfoReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<LobbyInfo>((LobbyInfo)convertedPacket!));
                break;
            case PacketId.CarDamage:
                CarDamageReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<CarDamage>((CarDamage)convertedPacket!));
                break;
            case PacketId.SessionHistory:
                SessionHistoryReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<SessionHistory>((SessionHistory)convertedPacket!));
                break;
        }
    }
}
