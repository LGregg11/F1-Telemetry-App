namespace F1GameTelemetry.Models;


public struct Motion
{
    public Motion(
        CarMotionData[] carMotionData,
        ExtraCarMotionData extraCarMotionData)
    {
        this.carMotionData = carMotionData;
        this.extraCarMotionData = extraCarMotionData;
    }

    public CarMotionData[] carMotionData;
    public ExtraCarMotionData extraCarMotionData;
}

public struct CarMotionData
{
    public CarMotionData(
        Vector3d worldPosition,
        Vector3d worldVelocity)
    {
        this.worldPosition = worldPosition;
        this.worldVelocity = worldVelocity;
    }

    public Vector3d worldPosition;
    public Vector3d worldVelocity;
}

public struct ExtraCarMotionData
{
    public ExtraCarMotionData(Vector3d localVelocity)
    {
        this.localVelocity = localVelocity;
    }

    public Vector3d localVelocity;
}

public struct Vector3d
{
    public Vector3d(float[] xyz) : this(xyz[0], xyz[1], xyz[2])
    {

    }

    public Vector3d(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float x;
    public float y;
    public float z;
}