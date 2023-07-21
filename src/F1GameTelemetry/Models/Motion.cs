namespace F1GameTelemetry.Models;


public struct Motion
{
    public Motion(
        CarMotionData[] carMotionData)
    {
        this.carMotionData = carMotionData;
    }

    public CarMotionData[] carMotionData;
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