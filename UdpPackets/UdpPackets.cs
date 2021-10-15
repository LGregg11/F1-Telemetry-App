using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpPackets
{
    #region Structs
    public struct PacketHeader
    {
        ushort packetFormat; // 2021
        byte gameMajorVersion; // Game major version - "X.00"
        byte gameMinorVersion; // Game minor version - "1.XX"
        byte packetVersion; // Version of this packet type, all start from 1
        byte packetId; // Identifier for the packet type
        byte sessionUID; // Unique identifier for the session
        float sessionTime; // Session timestamp
        uint frameIdentifier; // Identifier for the fram the data was retrieved on
        byte playerCarIndex; // Index of player's car in the array
        byte secondaryPlayerCarIndex; // Index of secondary player's car in the array (splitscreen) - 255 if no second player
    }
    #endregion

    #region Enums
    public enum PacketIds
    {
        // Contains all motion data for player's car - only sent while player is in control
        Motion = 0,

        // Data about the session - track, time left
        Session = 1,

        // Data about all the lap times of cars in the session
        LapData = 2,

        // Various notable events that happen during a session
        Event = 3,

        // List of participants in the session, mostly relevant for multiplayer
        Participants = 4,

        // Packet detailing car setups for cars in the race
        CarSetups = 5,

        // Telemetry data for all cars
        CarTelemetry = 6,

        // Status data for all cars
        CarStatus = 7,

        // Final classification confirmation at the end of a race
        FinalClassification = 8,

        // Information about players in a multiplayer lobby
        LobbyInfo = 9,

        // Damage status for all cars
        CarDamage = 10,

        // Lap and tyre data for session
        SessionHistory = 11
    }
    #endregion
}
