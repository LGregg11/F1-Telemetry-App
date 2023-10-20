namespace F1GameTelemetry.Models
{
    using Enums;

    public struct Header
    {
        public Header(
            PacketId packetId,
            ulong sessionUID,
            float sessionTime,
            uint frameIdentifier,
            byte playerCarIndex)
        {
            this.packetId = packetId;
            this.sessionUID = sessionUID;
            this.sessionTime = sessionTime;
            this.frameIdentifier = frameIdentifier;
            this.playerCarIndex = playerCarIndex;
        }

        public PacketId packetId; // Identifier for the packet type
        public ulong sessionUID; // Unique identifier for the session
        public float sessionTime; // Session timestamp
        public uint frameIdentifier; // Identifier for the frame the data was retrieved on
        public byte playerCarIndex; // Index of player's car in the array
    }
}
