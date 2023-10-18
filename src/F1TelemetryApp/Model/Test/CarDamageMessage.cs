namespace F1TelemetryApp.Model;

using F1GameTelemetry.Models;

public struct CarDamageMessage
{
    public FourAxleFloat TyreWear { get; set; }
    public int FrontLeftWingDamage { get; set; }
    public int FrontRightWingDamage { get; set; }
}
