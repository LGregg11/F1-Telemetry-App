namespace F1GameTelemetry.Events;

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
    public PacketEventArgs(T packet)
    {
        Packet = packet;
    }

    public T Packet { get; init; }
}

public delegate void PacketEventHandler<T>(object sender, PacketEventArgs<T> e);
