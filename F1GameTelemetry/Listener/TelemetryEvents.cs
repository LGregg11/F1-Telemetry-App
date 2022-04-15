namespace F1GameTelemetry.Listener
{
    using F1GameTelemetry.Packets.F12021;
    using System;

    public class TelemetryEventArgs : EventArgs
    {
        private byte[] message;
        public TelemetryEventArgs(byte[] message)
        {
            this.message = message;
        }
        public byte[] Message => message;
    }

    public delegate void TelemetryEventHandler(object source, TelemetryEventArgs e);

    public class HeaderEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public Header Header { get; set; }
    }

    public delegate void HeaderEventHandler(object sender, HeaderEventArgs e);

    public class MotionEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public Motion Motion { get; set; }
    }

    public delegate void MotionEventHandler(object sender, MotionEventArgs e);

    public class CarTelemetryEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public CarTelemetry CarTelemetry { get; set; }
    }

    public delegate void CarTelemetryEventHandler(object sender, CarTelemetryEventArgs e);

    public class CarStatusEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public CarStatus CarStatus { get; set; }
    }

    public delegate void CarStatusEventHandler(object sender, CarStatusEventArgs e);

    public class FinalClassificationEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public FinalClassification FinalClassification { get; set; }
    }

    public delegate void FinalClassificationEventHandler(object sender, FinalClassificationEventArgs e);

    public class LapDataEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public LapData LapData { get; set; }
    }

    public delegate void LapDataEventHandler(object sender, LapDataEventArgs e);

    public class SessionEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public Session Session { get; set; }
    }

    public delegate void SessionEventHandler(object sender, SessionEventArgs e);

    public class ParticipantEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public Participant Participant { get; set; }
    }

    public delegate void ParticipantEventHandler(object sender, ParticipantEventArgs e);

    public class SessionHistoryEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public SessionHistory SessionHistory { get; set; }
    }

    public delegate void SessionHistoryEventHandler(object sender, SessionHistoryEventArgs e);

    public class LobbyInfoEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public LobbyInfo LobbyInfo { get; set; }
    }

    public delegate void LobbyInfoEventHandler(object sender, LobbyInfoEventArgs e);

    public class CarDamageEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public CarDamage CarDamage { get; set; }
    }

    public delegate void CarDamageEventHandler(object sender, CarDamageEventArgs e);

    public class CarSetupEventArgs : EventArgs
    {
        // All telemetry readers should convert their data into the F12021 struct type (for simplicity)
        public CarSetup CarSetup { get; set; }
    }

    public delegate void CarSetupEventHandler(object sender, CarSetupEventArgs e);
}
