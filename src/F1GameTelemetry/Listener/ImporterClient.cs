namespace F1GameTelemetry.Listener;

using System;
using System.IO;
using System.Linq;
using System.Net;

internal class ImporterClient : IUdpClient
{
    private readonly StreamReader _streamReader;

    public ImporterClient(string filepath)
    {
        _streamReader = new StreamReader(File.OpenRead(filepath), null, true, -1, true);
    }

    public void Close()
    {
        // Do nothing
    }

    public void Dispose()
    {
        // Do nothing
    }

    public byte[]? Receive(ref IPEndPoint ep)
    {
        string? line = _streamReader.ReadLine();
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
