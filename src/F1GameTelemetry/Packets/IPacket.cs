namespace F1GameTelemetry.Packets;

using Events;

public interface IPacket<T>
{
    event PacketEventHandler<T> Received;
    void ReceivePacket(byte[] remainingPacket);
}
