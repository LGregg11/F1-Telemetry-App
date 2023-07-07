namespace F1GameTelemetry.Packets;

using F1GameTelemetry.Events;

public interface IPacket<T>
{
    event PacketEventHandler<T> Received;
    void ReceivePacket(byte[] remainingPacket);
}
