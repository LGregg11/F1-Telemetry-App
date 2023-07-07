namespace F1GameTelemetry.Packets;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Events;
using System;

public class Packet<T> : IPacket<T>
    where T : struct
{
    public event PacketEventHandler<T>? Received;

    public void ReceivePacket(byte[] remainingPacket) => 
        Received?.Invoke(this, new PacketEventArgs<T>(Converter.BytesToPacket<T>(remainingPacket)));
}
