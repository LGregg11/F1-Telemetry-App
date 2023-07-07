namespace F1GameTelemetry.Listener;

using Events;

using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TelemetryListener : ITelemetryListener
{
    private readonly int _port;
    private IUdpClient? _client;
    private Thread? _listenerThread;

    public TelemetryListener(int port) : this(port, null)
    {
    }

    public TelemetryListener(int port, IUdpClient? client)
    {
        if (client == null)
            client = CreateClient();

        _port = port;
        _client = client;
        _listenerThread = CreateThread();
    }

    public event TelemetryEventHandler? TelemetryReceived;

    public bool IsListenerRunning => _listenerThread != null && _listenerThread.IsAlive;

    public void Start()
    {
        if (IsListenerRunning)
            return;

        if (_client == null)
            _client = CreateClient();

        _listenerThread = CreateThread();
        _listenerThread.Start();
    }

    public void Stop()
    {
        if (!IsListenerRunning)
            return;

        _client?.Close();
        _listenerThread?.Join();
        _client = null;
    }

    public virtual void TelemetrySubscriber()
    {
        IPEndPoint ep = new(IPAddress.Any, _port);

        while (true)
        {
            try
            {
                byte[]? receiveBytes = _client?.Receive(ref ep);
                if (receiveBytes != null && receiveBytes.Length > 0)
                    TelemetryReceived?.Invoke(this, new TelemetryEventArgs(receiveBytes));
            }
            catch (SocketException)
            {
                // Client closed - stop the thread
                break;
            }
        }
    }

    public Thread CreateThread() => new(new ThreadStart(TelemetrySubscriber))
    {
        Name = "Telemetry Listener Thread"
    };

    public IUdpClient CreateClient() => new TelemetryUdpClient(_port);
}
