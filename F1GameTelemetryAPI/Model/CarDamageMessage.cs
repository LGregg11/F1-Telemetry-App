namespace F1GameTelemetryAPI.Model;

using Google.Cloud.Firestore;
using Interfaces;

[FirestoreData]
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

    [FirestoreProperty]
    public string Id { get; set; }

    [FirestoreProperty]
    public float[] TyreWear { get; set; }

    [FirestoreProperty]
    public int FrontLeftWingDamage { get; set; }

    [FirestoreProperty]
    public int FrontRightWingDamage { get; set; }
}
