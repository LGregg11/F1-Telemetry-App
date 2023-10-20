namespace F1GameTelemetry.Listener
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

#nullable enable

    internal class ImporterClient : IUdpClient
    {
        private const int DELAY_MS = 3;
        private readonly string _filepath;
        private StreamReader? _streamReader;

        public ImporterClient(string filepath)
        {
            _filepath = filepath;
            _streamReader = new StreamReader(File.OpenRead(_filepath), Encoding.UTF8);
        }

        public void Close()
        {
            _streamReader = null;
        }

        public void Dispose()
        {
            _streamReader = null;
        }

        public byte[]? Receive(ref IPEndPoint ep)
        {
            // If set to null, client has been closed - reset _streamReader ahead of next 'Start' and throw an exception
            if (_streamReader == null)
            {
                _streamReader = new StreamReader(File.OpenRead(_filepath), null, true, -1, true);
                throw new SocketException();
            }

            // Very short delay to try and make the rate of receipt more realistic (otherwise it is like 20x speed..)
            Thread.Sleep(DELAY_MS);

            string? line = _streamReader?.ReadLine();
            if (line == null)
                return null;

            // Line should have format '{ 0, 1, 2, 3, 4, 5 }' - we want this to look like '0 1 2 3 4 5'
            // From there we can then convert the string to a list of ints, and from there use GetBytes()
            int[] byteInts = line.Replace("{", "").Replace("}", "").Trim()
                .Split(", ")
                .Select(s => int.Parse(s)).ToArray();
            byte[] bytes = byteInts.Select(i => (byte)i).ToArray();
            return bytes;
        }
    }
}
