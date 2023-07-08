namespace F1GameTelemetry.Packets;

using Converters;
using Events;

using System;

public class Packet<T> : IPacket<T>
    where T : struct
{
    public event PacketEventHandler<T>? Received;

    public void ReceivePacket(byte[] remainingPacket) => 
        Received?.Invoke(this, new PacketEventArgs<T>(Converter.BytesToPacket<T>(remainingPacket)));
}
