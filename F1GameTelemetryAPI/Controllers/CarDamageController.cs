namespace F1GameTelemetryAPI.Controllers;

using F1GameTelemetry.Packets.F12021;
using Model;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CarDamageController : ControllerBase
{

    private readonly ILogger<CarDamageController> _logger;

    public CarDamageController(ILogger<CarDamageController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets the Car Damage.
    /// Live data should be pulled directly from the realtime database directly on the app.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "Car Damage")]
    public async Task<ActionResult<IEnumerable<CarDamageMessage>>> Get()
    {
        var list = new List<CarDamageMessage>{
            new CarDamageMessage
            {
                TyreWear = new float[4]
                {
                    10.07f,
                    11.56f,
                    10.35f,
                    12.74f
                },
                FrontLeftWingDamage = 10,
                FrontRightWingDamage = 2,
            }
        };

        return Ok(list);
    }
}
