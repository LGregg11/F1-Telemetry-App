namespace F1GameTelemetry.Packets.Standard;

using F1GameTelemetry.Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1167)]
public struct LobbyInfo
{
    public LobbyInfo(byte numPlayers, LobbyInfoData[] lobbyPlayers)
    {
        this.numPlayers = numPlayers;
        this.lobbyPlayers = lobbyPlayers;
    }

    public byte numPlayers;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public LobbyInfoData[] lobbyPlayers;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 53)]
public struct LobbyInfoData
{
    public LobbyInfoData(
        AiControlled aiControlled,
        Team teamId,
        Nationality nationality,
        string name,
        byte carNumber,
        ReadyStatus readyStatus)
    {
        this.aiControlled = aiControlled;
        this.teamId = teamId;
        this.nationality = nationality;
        this.name = name;
        this.carNumber = carNumber;
        this.readyStatus = readyStatus;
    }

    public AiControlled aiControlled;
    public Team teamId;
    public Nationality nationality;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
    public string name; // Will be truncated with '...' (U+2026) if too long

    public byte carNumber;
    public ReadyStatus readyStatus;
}
