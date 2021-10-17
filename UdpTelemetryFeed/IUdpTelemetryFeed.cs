﻿namespace UdpTelemetryFeed
{
    using System.Net.Sockets;
    using System.Threading;

    public interface IUdpTelemetryFeed
    {
        event UdpTelemetryEventHandler TelemetryReceived;
        int Port { get; }
        Thread ListenerThread { get; }
        UdpClient Client { get; }
        void Start();
        void Stop();
        void TelemetryListener();
    }
}