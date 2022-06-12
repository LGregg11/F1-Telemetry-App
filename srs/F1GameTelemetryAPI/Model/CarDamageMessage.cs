namespace F1GameTelemetryAPI.Model;

using Interfaces;

[Serializable]
public class CarDamageMessage : IFirebaseEntity
{
    public CarDamageMessage() : this(Guid.NewGuid().ToString("N"))
    {
    }

    public CarDamageMessage(string id)
    {
        Id = id;
        TyreWear = new float[4] { 0f, 0f, 0f, 0f };
        FrontLeftWingDamage = 0;
        FrontRightWingDamage = 0;
    }

    public string Id { get; set; }
    public float[] TyreWear { get; set; }
    public int FrontLeftWingDamage { get; set; }
    public int FrontRightWingDamage { get; set; }
}
