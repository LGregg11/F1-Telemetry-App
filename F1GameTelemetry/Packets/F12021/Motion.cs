namespace F1GameTelemetry.Packets.F12021
{
    using F1GameTelemetry.Converters;
    using F1GameTelemetry.Listener;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1436)]
    public struct Motion
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public CarMotionData[] carMotionData;

        public ExtraCarMotionData extraCarMotionData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 60)]
    public struct CarMotionData
    {
        // Spec says to divide normalised vectors by 32767.0f to convert to floats
        // (assumes that direction values are always between -1.0f and 1.0f)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] worldPosition; // x, y, z

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] worldVelocity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] worldForwardDir; // Normalised

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] worldRightDir; // Normalised

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] gForce; // lat, lon, vert

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] rotation; // roll, pitch, yaw
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 116)]
    public struct ExtraCarMotionData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] suspensionPosition; // rear left, rear right, front left, front right

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] suspensionVelocity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] suspensionAcceleration;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelSpeed;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] wheelSlip;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] localVelocity; // x, y, z

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] angularVelocity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] angularAcceleration;
    }

    public class MotionPacket : IPacket
    {
        public event EventHandler? Received;

        public void ReceivePacket(byte[] remainingPacket)
        {
            var args = new MotionEventArgs
            {
                Motion = Converter.BytesToPacket<Motion>(remainingPacket)
            };

            Received?.Invoke(this, args);
        }
    }
}
