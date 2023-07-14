namespace F1GameTelemetry.Packets.F12023;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 29)]
public struct Header
{
    public GameVersion packetFormat; // 2023
    public byte gameYear; // Game year - last to digits e.g. 23
    public byte gameMajorVersion; // Game major version - "X.00"
    public byte gameMinorVersion; // Game minor version - "1.XX"
    public byte packetVersion; // Version of this packet type, all start from 1
    public PacketId packetId; // Identifier for the packet type
    public ulong sessionUID; // Unique identifier for the session
    public float sessionTime; // Session timestamp
    public uint frameIdentifier; // Identifier for the frame the data was retrieved on
    public uint overallFrameIdentifier; // Overall identifier for the fram the data was retrieved on (doesn't go back after flashbacks)
    public byte playerCarIndex; // Index of player's car in the array
    public byte secondaryPlayerCarIndex; // Index of secondary player's car in the array (splitscreen) - 255 if no second player
}
