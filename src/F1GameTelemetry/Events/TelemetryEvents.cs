namespace F1GameTelemetry.Events;

using Models;

using System;

public class TelemetryEventArgs : EventArgs
{
    public TelemetryEventArgs(byte[] message)
    {
        Message = message;
    }

    public byte[] Message { get; init; }
}

public delegate void TelemetryEventHandler(object source, TelemetryEventArgs e);

public class PacketEventArgs<T> : EventArgs
{
    public PacketEventArgs(Header header, T packet)
    {
        Header = header;
        Packet = packet;
    }

    public Header Header { get; init; }
    public T Packet { get; init; }
}

public delegate void PacketEventHandler<T>(object sender, PacketEventArgs<T> e);
