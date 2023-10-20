namespace F1GameTelemetry.Events
{
    using Models;

    using System;

    public class TelemetryEventArgs : EventArgs
    {
        public TelemetryEventArgs(byte[] message)
        {
            Message = message;
        }

        public byte[] Message { get; private set; }
    }

    public delegate void TelemetryEventHandler(object source, TelemetryEventArgs e);

    public class PacketEventArgs<T> : EventArgs
    {
        public PacketEventArgs(Header header, T packet)
        {
            Header = header;
            Packet = packet;
        }

        public Header Header { get; private set; }
        public T Packet { get; private set; }
    }

    public delegate void PacketEventHandler<T>(object sender, PacketEventArgs<T> e);
}
