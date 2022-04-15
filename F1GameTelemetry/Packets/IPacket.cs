namespace F1GameTelemetry.Packets
{
    using System;

    public interface IPacket
    {
        void ReceivePacket(byte[] remainingPacket);
        event EventHandler Received;
    }
}
