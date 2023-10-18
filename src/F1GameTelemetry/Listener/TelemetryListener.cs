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
    private bool _keepThreadRunning;

    public TelemetryListener(int port) : this(port, null)
    {
    }

    public TelemetryListener(int port, IUdpClient? client)
    {
        _port = port;

        if (client == null)
            client = CreateClient();
        _client = client;

        _keepThreadRunning = true;
    }

    public event TelemetryEventHandler? TelemetryReceived;

    public bool IsListenerRunning => _listenerThread != null && _listenerThread.IsAlive;

    public void Start()
    {
        if (IsListenerRunning)
            return;

        if (_client == null)
            _client = CreateClient();

        _keepThreadRunning = true;
        _listenerThread = CreateThread();
        _listenerThread.Start();
    }

    public void Stop()
    {
        if (!IsListenerRunning)
            return;

        _client?.Close();
        _keepThreadRunning = false;
        _listenerThread?.Join();
        _client = null;
    }

    public virtual void TelemetrySubscriber()
    {
        IPEndPoint ep = new(IPAddress.Any, _port);

        while (_keepThreadRunning)
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

    public Thread CreateThread() => new(TelemetrySubscriber)
    {
        Name = "Telemetry Listener Thread"
    };

    public IUdpClient CreateClient() => new TelemetryUdpClient(_port);
}
