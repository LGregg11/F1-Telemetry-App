namespace F1GameTelemetry.Listener;

using System.Net;

public interface IUdpClient
{
    void Close();
    void Dispose();
    byte[]? Receive(ref IPEndPoint ep);
}
