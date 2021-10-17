namespace UdpPackets
{
    using System.Runtime.InteropServices;

    #region Structs

    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 24)]
    public struct UdpPacketHeader
    {
        [FieldOffset(0)]
        public ushort packetFormat; // 2021

        [FieldOffset(2)]
        public byte gameMajorVersion; // Game major version - "X.00"

        [FieldOffset(3)]
        public byte gameMinorVersion; // Game minor version - "1.XX"

        [FieldOffset(4)]
        public byte packetVersion; // Version of this packet type, all start from 1

        [FieldOffset(5)]
        public byte packetId; // Identifier for the packet type

        [FieldOffset(6)]
        public ulong sessionUID; // Unique identifier for the session

        [FieldOffset(14)]
        public float sessionTime; // Session timestamp

        [FieldOffset(18)]
        public uint frameIdentifier; // Identifier for the fram the data was retrieved on

        [FieldOffset(22)]
        public byte playerCarIndex; // Index of player's car in the array

        [FieldOffset(23)]
        public byte secondaryPlayerCarIndex; // Index of secondary player's car in the array (splitscreen) - 255 if no second player
    }
    #endregion
}
