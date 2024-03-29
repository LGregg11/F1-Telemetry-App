﻿namespace F1GameTelemetry.Readers;

using Converters;
using Enums;
using Events;
using Models;
using Listener;

using System.Linq;
using System.Threading.Tasks;

public static class SingletonTelemetryReader
{

    private static ITelemetryListener? _telemetryListener;
    private static ITelemetryConverter? _telemetryConverter;

    public static event PacketEventHandler<Motion>? MotionReceived;
    public static event PacketEventHandler<Session>? SessionReceived;
    public static event PacketEventHandler<LapData>? LapDataReceived;
    public static event PacketEventHandler<Participant>? ParticipantReceived;
    //public static event PacketEventHandler<CarSetup>? CarSetupReceived;
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

    public static void SetTelemetryConverterByVersion(ITelemetryConverter converter)
    {
        _telemetryConverter = converter;
    }

    private static void OnTelemetryReceived(object sender, TelemetryEventArgs e)
    {
        if (_telemetryConverter == null)
            return;

        Header header = _telemetryConverter!.ConvertBytesToHeader(e.Message);
        // Trace.WriteLine($"--> Frame id: {header.frameIdentifier} Packet type: {Enum.GetName(header.packetId)}");

        byte[] remainingPacket = e.Message.Skip(_telemetryConverter.PacketHeaderSize).ToArray();
        Task remainingTask = new(() => ConvertAndRaiseEvent(header, remainingPacket));
        remainingTask.RunSynchronously();
    }

    private static void ConvertAndRaiseEvent(Header header, byte[] packet)
    {
        object? convertedPacket = _telemetryConverter?.ConvertBytesToStandardPacket(header.packetId, packet);

        switch (header.packetId)
        {
            case PacketId.Motion:
                MotionReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<Motion>(header, (Motion)convertedPacket!));
                break;
            case PacketId.Session:
                SessionReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<Session>(header, (Session)convertedPacket!));
                break;
            case PacketId.LapData:
                LapDataReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<LapData>(header, (LapData)convertedPacket!));
                break;
            case PacketId.Event:
                // TODO: Events
                break;
            case PacketId.Participant:
                ParticipantReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<Participant>(header, (Participant)convertedPacket!));
                break;
            case PacketId.CarSetup:
                //CarSetupReceived?.Invoke(
                //    typeof(SingletonTelemetryReader),
                //    new PacketEventArgs<CarSetup>((CarSetup)convertedPacket!));
                break;
            case PacketId.CarTelemetry:
                CarTelemetryReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<CarTelemetry>(header, (CarTelemetry)convertedPacket!));
                break;
            case PacketId.CarStatus:
                CarStatusReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<CarStatus>(header, (CarStatus)convertedPacket!));
                break;
            case PacketId.FinalClassification:
                FinalClassificationReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<FinalClassification>(header, (FinalClassification)convertedPacket!));
                break;
            case PacketId.LobbyInfo:
                LobbyInfoReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<LobbyInfo>(header, (LobbyInfo)convertedPacket!));
                break;
            case PacketId.CarDamage:
                CarDamageReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<CarDamage>(header, (CarDamage)convertedPacket!));
                break;
            case PacketId.SessionHistory:
                SessionHistoryReceived?.Invoke(
                    typeof(SingletonTelemetryReader),
                    new PacketEventArgs<SessionHistory>(header, (SessionHistory)convertedPacket!));
                break;
        }
    }
}
