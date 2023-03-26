namespace F1GameTelemetry.Listener;

using System;
using System.Net;
using System.Net.Sockets;

internal class TelemetryUdpClient : IUdpClient, IDisposable
{
    private UdpClient _client;

    public TelemetryUdpClient(int port)
    {
        _client = new UdpClient(port);
    }

    public void Close()
    {
        _client.Close();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public byte[]? Receive(ref IPEndPoint ep)
    {
        try
        {
            return _client.Receive(ref ep);
        }
        catch (SocketException)
        {
            // Client closed - stop the thread
            throw;
        }
    }
}
