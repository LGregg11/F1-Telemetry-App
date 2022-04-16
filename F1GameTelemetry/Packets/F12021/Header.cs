namespace F1GameTelemetry.Packets.F12021
{
    using F1GameTelemetry.Converters;
    using F1GameTelemetry.Enums;
    using F1GameTelemetry.Listener;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 24)]
    public struct Header
    {
        public ReaderVersion packetFormat; // 2021

        public byte gameMajorVersion; // Game major version - "X.00"

        public byte gameMinorVersion; // Game minor version - "1.XX"

        public byte packetVersion; // Version of this packet type, all start from 1

        public byte packetId; // Identifier for the packet type

        public ulong sessionUID; // Unique identifier for the session

        public float sessionTime; // Session timestamp

        public uint frameIdentifier; // Identifier for the fram the data was retrieved on

        public byte playerCarIndex; // Index of player's car in the array

        public byte secondaryPlayerCarIndex; // Index of secondary player's car in the array (splitscreen) - 255 if no second player
    }

    public class HeaderPacket : IPacket
    {
        public event EventHandler? Received;

        public void ReceivePacket(byte[] remainingPacket)
        {
            var args = new HeaderEventArgs
            {
                Header = Converter.BytesToPacket<Header>(remainingPacket)
            };

            Received?.Invoke(this, args);
        }
    }
}
