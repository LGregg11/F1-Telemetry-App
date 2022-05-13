namespace F1GameTelemetryAPI.Controllers;

using Helper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(ILogger<HealthCheckController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetListenerStatus")]
    public bool GetListenerStatus()
    {
        var listener = TelemetryListenerHelper.TelementryListener;
        if (listener == null)
            return false;
        return listener.IsListenerRunning;
    }
}
