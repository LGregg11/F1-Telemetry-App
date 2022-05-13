namespace F1GameTelemetryAPI.Controllers;

using Model;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class LiveSessionController : ControllerBase
{

    private readonly ILogger<LiveSessionController> _logger;

    public LiveSessionController(ILogger<LiveSessionController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetCarDamage")]
    public CarDamageMessage GetCarDamage()
    {
        return new CarDamageMessage();
    }
}
