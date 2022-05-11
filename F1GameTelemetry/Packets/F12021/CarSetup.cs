namespace F1GameTelemetry.Packets.F12021;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Listener;
using System;
using System.Runtime.InteropServices;

// Other players cars will appear as blank (Unless AI)
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1078)]
public struct CarSetup
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public CarSetupData[] carSetupData;
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 49)]
public struct CarSetupData
{
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

public class CarSetupPacket : IPacket
{
    public event EventHandler? Received;

    public void ReceivePacket(byte[] remainingPacket)
    {
        var args = new CarSetupEventArgs
        {
            CarSetup = Converter.BytesToPacket<CarSetup>(remainingPacket)
        };

        Received?.Invoke(this, args);
        
    }
}
