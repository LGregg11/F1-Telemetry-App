namespace F1GameTelemetry.Listener
{
    using System.Net;

#nullable enable

    public interface IUdpClient
    {
        void Close();
        void Dispose();
        byte[]? Receive(ref IPEndPoint ep);
    }
}
