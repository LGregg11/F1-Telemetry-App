namespace F1GameTelemetry.Models;

using Enums;


public struct Participant
{
    public Participant(
        byte numActiveCars,
        ParticipantData[] participants)
    {
        this.numActiveCars = numActiveCars;
        this.participants = participants;
    }

    public byte numActiveCars;
    public ParticipantData[] participants;
}

public struct ParticipantData
{
    public ParticipantData(
        DriverId driverId,
        Team teamId,
        byte raceNumber,
        Nationality nationality,
        string name)
    {
        this.driverId = driverId;
        this.teamId = teamId;
        this.raceNumber = raceNumber;
        this.nationality = nationality;
        this.name = name;
    }

    public DriverId driverId; // Driver IDs (255 for Human)
    public Team teamId;
    public byte raceNumber;
    public Nationality nationality;
    public string name; // UTF-8 format
}
