namespace F1GameTelemetry.Packets.Standard;

using Enums;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1233)]
public struct Participant
{
    public Participant(byte numActiveCars, ParticipantData[] participants)
    {
        this.numActiveCars = numActiveCars;
        this.participants = participants;
    }

    public byte numActiveCars;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public ParticipantData[] participants;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 56)]
public struct ParticipantData
{
    public ParticipantData(
        AiControlled aiControlled,
        DriverId driverId,
        byte networkId,
        Team teamId,
        MyTeam myTeam,
        byte raceNumber,
        Nationality nationality,
        string name,
        UdpSetting yourTelemetry)
    {
        this.aiControlled = aiControlled;
        this.driverId = driverId;
        this.networkId = networkId;
        this.teamId = teamId;
        this.myTeam = myTeam;
        this.raceNumber = raceNumber;
        this.nationality = nationality;
        this.name = name;
        this.yourTelemetry = yourTelemetry;
    }

    // "The array should be indexed by vehicle index"

    public AiControlled aiControlled;
    public DriverId driverId; // Driver IDs (255 for Human)
    public byte networkId;
    public Team teamId;
    public MyTeam myTeam;
    public byte raceNumber;
    public Nationality nationality;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
    public string name; // UTF-8 format

    public UdpSetting yourTelemetry;
}
