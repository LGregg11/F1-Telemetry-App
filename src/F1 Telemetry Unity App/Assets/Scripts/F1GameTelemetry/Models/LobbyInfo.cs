namespace F1GameTelemetry.Models
{
    using Enums;

    public struct LobbyInfo
    {
        public LobbyInfo(
            byte numPlayers,
            LobbyInfoData[] lobbyPlayers)
        {
            this.numPlayers = numPlayers;
            this.lobbyPlayers = lobbyPlayers;
        }

        public byte numPlayers;
        public LobbyInfoData[] lobbyPlayers;
    }

    public struct LobbyInfoData
    {
        public LobbyInfoData(
            AiControlled aiControlled,
            Team teamId,
            Nationality nationality,
            string name)
        {
            this.aiControlled = aiControlled;
            this.teamId = teamId;
            this.nationality = nationality;
            this.name = name;
        }

        public AiControlled aiControlled;
        public Team teamId;
        public Nationality nationality;
        public string name;
    }
}
