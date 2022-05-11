namespace F1GameTelemetryAPI.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SessionHistory : ControllerBase
{

    private readonly ILogger<SessionHistory> _logger;

    public SessionHistory(ILogger<SessionHistory> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets the Session History.
    /// Live data should be pulled directly from the realtime database directly on the app.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "Driver")]
    public IEnumerable<SessionHistory> Get()
    {
        return null;
    }
}
