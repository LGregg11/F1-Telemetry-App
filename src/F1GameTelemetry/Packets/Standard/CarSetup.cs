namespace F1GameTelemetry.Packets.Standard;

using System.Runtime.InteropServices;

// Other players cars will appear as blank (Unless AI)
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1078)]
public struct CarSetup
{
    public CarSetup(CarSetupData[] carSetupData)
    {
        this.carSetupData = carSetupData;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarSetupData[] carSetupData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 49)]
public struct CarSetupData
{
    public CarSetupData(
        byte frontWing,
        byte rearWing,
        byte onThrottle,
        byte offThrottle,
        float frontCamber, 
        float rearCamber, 
        float frontToe, 
        float rearToe, 
        byte frontSuspension,
        byte rearSuspension,
        byte frontAntiRollBar,
        byte rearAntiRollBar,
        byte frontSuspensionHeight,
        byte rearSuspensionHeight,
        byte brakePressure, 
        byte brakeBias, 
        float rearLeftTyrePressure, 
        float rearRightTyrePressure, 
        float frontLeftTyrePressure, 
        float frontRightTyrePressure, 
        byte ballast,
        float fuelLoad)
    {
        this.frontWing = frontWing;
        this.rearWing = rearWing;
        this.onThrottle = onThrottle;
        this.offThrottle = offThrottle;
        this.frontCamber = frontCamber;
        this.rearCamber = rearCamber;
        this.frontToe = frontToe;
        this.rearToe = rearToe;
        this.frontSuspension = frontSuspension;
        this.rearSuspension = rearSuspension;
        this.frontAntiRollBar = frontAntiRollBar;
        this.rearAntiRollBar = rearAntiRollBar;
        this.frontSuspensionHeight = frontSuspensionHeight;
        this.rearSuspensionHeight = rearSuspensionHeight;
        this.brakePressure = brakePressure;
        this.brakeBias = brakeBias;
        this.rearLeftTyrePressure = rearLeftTyrePressure;
        this.rearRightTyrePressure = rearRightTyrePressure;
        this.frontLeftTyrePressure = frontLeftTyrePressure;
        this.frontRightTyrePressure = frontRightTyrePressure;
        this.ballast = ballast;
        this.fuelLoad = fuelLoad;
    }

    public byte frontWing;
    public byte rearWing;
    public byte onThrottle; // Percentage
    public byte offThrottle; // Percentage
    public float frontCamber; // suspension geometry (?)
    public float rearCamber; // suspension geometry (?)
    public float frontToe; // suspension geometry (?)
    public float rearToe; // suspension geometry (?)
    public byte frontSuspension;
    public byte rearSuspension;
    public byte frontAntiRollBar;
    public byte rearAntiRollBar;
    public byte frontSuspensionHeight;
    public byte rearSuspensionHeight;
    public byte brakePressure; // Percentage
    public byte brakeBias; // Percentage
    public float rearLeftTyrePressure; // PSI
    public float rearRightTyrePressure; // PSI
    public float frontLeftTyrePressure; // PSI
    public float frontRightTyrePressure; // PSI
    public byte ballast;
    public float fuelLoad; // Litres? Laps?
}
