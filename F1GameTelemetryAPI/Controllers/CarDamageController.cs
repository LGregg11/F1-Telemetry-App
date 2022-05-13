namespace F1GameTelemetryAPI.Controllers;

using Model;
using Providers;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;

[ApiController]
[Route("api/[controller]")]
public class CarDamageController : ControllerBase
{
    private FirestoreProvider _db;

    private readonly ILogger<CarDamageController> _logger;

    public CarDamageController(IServiceProvider services, ILogger<CarDamageController> logger)
    {
        _db = new FirestoreProvider(services.GetRequiredService<FirestoreDb>());
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
        var result = await _db.GetAll<CarDamageMessage>(CancellationToken.None);
        return Ok(result);
    }

    /// <summary>
    /// Post the Car Damage.
    /// Live data should be pulled directly from the realtime database directly on the app.
    /// </summary>
    /// <returns></returns>
    [HttpPost(Name = "Car Damage")]
    public async Task<ActionResult<IEnumerable<CarDamageMessage>>> AddCarDamageMessage([FromBody]string id)
    {
        var carDamageMessage = new CarDamageMessage(id);
        await _db.AddOrUpdate(carDamageMessage, CancellationToken.None);

        var result = await _db.GetAll<CarDamageMessage>(CancellationToken.None);

        return Ok(result);
    }
}
