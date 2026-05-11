using Microsoft.AspNetCore.Mvc;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Health check endpoint for Docker and monitoring
        /// </summary>
        [HttpGet]
        public ActionResult<object> Get()
        {
            _logger.LogInformation("Health check called");
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                service = "MedTrack API",
                version = "1.0.0"
            });
        }

        /// <summary>
        /// Liveness probe for Kubernetes
        /// </summary>
        [HttpGet("live")]
        public ActionResult Alive()
        {
            return Ok("alive");
        }

        /// <summary>
        /// Readiness probe for Kubernetes
        /// </summary>
        [HttpGet("ready")]
        public ActionResult Ready()
        {
            return Ok("ready");
        }
    }
}
