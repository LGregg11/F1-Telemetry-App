using F1TelemetryApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace F1GameTelemetryAPI.Controllers
{
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
        [HttpGet(Name = "GetCarDamage")]
        public IEnumerable<SessionHistory> Get()
        {

            return null;
        }
    }
}