namespace F1GameTelemetry.Models
{
    public struct FourAxleFloat
    {
        public FourAxleFloat(float[] values) : this(values[0], values[1], values[2], values[3])
        {
        }

        public FourAxleFloat(float rearLeft, float rearRight, float frontLeft, float frontRight)
        {
            this.rearLeft = rearLeft;
            this.rearRight = rearRight;
            this.frontLeft = frontLeft;
            this.frontRight = frontRight;
        }

        public float rearLeft;
        public float rearRight;
        public float frontLeft;
        public float frontRight;
    }

    public struct FourAxleUnsignedShort
    {
        public FourAxleUnsignedShort(ushort[] values) : this(values[0], values[1], values[2], values[3])
        {
        }

        public FourAxleUnsignedShort(ushort rearLeft, ushort rearRight, ushort frontLeft, ushort frontRight)
        {
            this.rearLeft = rearLeft;
            this.rearRight = rearRight;
            this.frontLeft = frontLeft;
            this.frontRight = frontRight;
        }

        public ushort rearLeft;
        public ushort rearRight;
        public ushort frontLeft;
        public ushort frontRight;
    }

    public struct FourAxleByte
    {
        public FourAxleByte(byte[] values) : this(values[0], values[1], values[2], values[3])
        {
        }

        public FourAxleByte(byte rearLeft, byte rearRight, byte frontLeft, byte frontRight)
        {
            this.rearLeft = rearLeft;
            this.rearRight = rearRight;
            this.frontLeft = frontLeft;
            this.frontRight = frontRight;
        }

        public byte rearLeft;
        public byte rearRight;
        public byte frontLeft;
        public byte frontRight;
    }
}
