namespace F1GameTelemetry.Converters
{
    using F1GameTelemetry.Models;

    using System;
    using System.Runtime.InteropServices;

    public static class Converter
    {
        public static T BytesToPacket<T>(byte[] remainingPacket)
        {
            GCHandle handle = GCHandle.Alloc(remainingPacket, GCHandleType.Pinned);
            T packet;
            try
            {
                packet = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T))!;
            }
            catch
            {
                throw new Exception($"Byte array failed to point to structure '{nameof(T)}'");
            }
            finally
            {
                handle.Free();
            }
            return packet;
        }

        public static double GetMagnitudeFromVectorData(float[] vector)
        {
            return Math.Round(Math.Sqrt(Math.Pow(vector[0], 2) + Math.Pow(vector[1], 2) + Math.Pow(vector[2], 2)), 3);
        }

        public static double GetMagnitudeFromVectorData(Vector3d vector)
        {
            return Math.Round(Math.Sqrt(Math.Pow(vector.x, 2) + Math.Pow(vector.y, 2) + Math.Pow(vector.z, 2)), 3);
        }

        /// <summary>
        /// Convert float to a standard telemetry time format.
        /// </summary>
        /// <param name="time">Time given in ms.</param>
        /// <returns>The time in a standard telemetry format.</returns>
        public static string ToTelemetryTime(this float milliSeconds)
        {
            if (milliSeconds <= 0)
                return "--:--:--";
            TimeSpan t = TimeSpan.FromMilliseconds(milliSeconds);
            string timeStr = string.Format("{0:D2}:{1:D2}:{2:D3}", t.Minutes, t.Seconds, t.Milliseconds);
            if (t.Hours > 0)
                timeStr = string.Format("{0:D2}:", t.Hours) + timeStr;
            return timeStr;
        }

        /// <summary>
        /// Convert ushort to a standard telemetry time format.
        /// </summary>
        /// <param name="time">Time given in seconds.</param>
        /// <returns>The time in a standard telemetry format.</returns>
        public static string ToTelemetryTime(this ushort seconds)
        {
            double time = Convert.ToDouble(seconds);
            if (time <= 0)
                return "--:--";
            TimeSpan t = TimeSpan.FromSeconds(time);
            string timeStr = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            if (t.Hours > 0)
                timeStr = string.Format("{0:D2}:", t.Hours) + timeStr;
            return timeStr;
        }

        public static string ToName(this char[] chars) => new string(chars);
    }
}
