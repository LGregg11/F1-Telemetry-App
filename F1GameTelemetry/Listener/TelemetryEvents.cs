namespace F1GameTelemetry.Listener
{
    using System;

    public delegate void TelemetryEventHandler(object source, TelemetryEventArgs e);

    public class TelemetryEventArgs : EventArgs
    {
        private byte[] message;
        public TelemetryEventArgs(byte[] message)
        {
            this.message = message;
        }
        public byte[] Message => message;
    }
}
