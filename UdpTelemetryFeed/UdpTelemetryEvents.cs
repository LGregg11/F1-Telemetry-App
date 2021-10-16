namespace UdpTelemetryFeed
{
    using System;

    public delegate void UdpTelemetryEventHandler(object source, UdpTelemetryEventArgs e);

    public class UdpTelemetryEventArgs : EventArgs
    {
        private byte[] message;
        public UdpTelemetryEventArgs(byte[] message)
        {
            this.message = message;
        }
        public byte[] GetMessage() => message;
    }
}
