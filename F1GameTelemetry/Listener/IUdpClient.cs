namespace F1GameTelemetry.Listener;

using System.Net;
using System.Net.Sockets;

public interface IUdpClient
{
    UdpClient Client { get; }
    void Close();
    void Dispose();
    byte[]? Receive(ref IPEndPoint ep);
}
