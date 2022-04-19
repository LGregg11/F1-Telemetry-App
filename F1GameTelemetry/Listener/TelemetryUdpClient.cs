namespace F1GameTelemetry.Listener;

using System;
using System.Net;
using System.Net.Sockets;

internal class TelemetryUdpClient : IUdpClient, IDisposable
{
    public TelemetryUdpClient(int port)
    {
        Client = new UdpClient(port);
    }

    public UdpClient Client { get; }

    public void Close()
    {
        Client.Close();
        Dispose();
    }

    public void Dispose()
    {
        Client.Dispose();
    }

    public byte[]? Receive(ref IPEndPoint ep)
    {
        try
        {
            return Client.Receive(ref ep);
        }
        catch (SocketException)
        {
            // Client closed - stop the thread
            throw;
        }
    }
}
