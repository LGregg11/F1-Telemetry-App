namespace F1GameTelemetry.Models;


public struct CarDamage
{
    public CarDamage(CarDamageData[] carDamageData)
    {
        this.carDamageData = carDamageData;
    }

    public CarDamageData[] carDamageData;
}

public struct CarDamageData
{
    public CarDamageData(
        FourAxleFloat tyreWear,
        FourAxleByte tyreDamage,
        FourAxleByte brakeDamage,
        byte frontLeftWingDamage,
        byte frontRightWingDamage,
        byte rearWingDamage)
    {
        this.tyreWear = tyreWear;
        this.tyreDamage = tyreDamage;
        this.brakeDamage = brakeDamage;
        this.frontLeftWingDamage = frontLeftWingDamage;
        this.frontRightWingDamage = frontRightWingDamage;
        this.rearWingDamage = rearWingDamage;
    }

    // All properties are percentage

    public FourAxleFloat tyreWear;
    public FourAxleByte tyreDamage;
    public FourAxleByte brakeDamage;

    public byte frontLeftWingDamage;
    public byte frontRightWingDamage;
    public byte rearWingDamage;
}