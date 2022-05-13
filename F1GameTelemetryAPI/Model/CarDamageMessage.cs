namespace F1GameTelemetryAPI.Model;

public struct CarDamageMessage
{
    public float[] TyreWear { get; set; }
    public int FrontLeftWingDamage { get; set; }
    public int FrontRightWingDamage { get; set; }
}
