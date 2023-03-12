namespace F1GameTelemetry.Packets.F12021;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Listener;
using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1167)]
public struct LobbyInfo
{
    public byte numPlayers;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public LobbyInfoData[] lobbyPlayers;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 53)]
public struct LobbyInfoData
{
    public AiControlled aiControlled;
    public Team teamId;
    public Nationality nationality;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
    public string name; // Will be truncated with '...' (U+2026) if too long

    public byte carNumber;
    public ReadyStatus readyStatus;
}

public class LobbyInfoPacket : IPacket
{
    public event EventHandler? Received;

    public void ReceivePacket(byte[] remainingPacket)
    {
        var args = new LobbyInfoEventArgs
        {
            LobbyInfo = Converter.BytesToPacket<LobbyInfo>(remainingPacket)
        };

        Received?.Invoke(this, args);
    }
}
