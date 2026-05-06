using Microsoft.AspNetCore.Mvc;

namespace irrigation_system.Controllers
{
    [ApiController]
    [Route("api/time")]
    public class TimeController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            var now = DateTimeOffset.Now;

            long unixTime = now.ToUnixTimeSeconds();
            int secondsSinceMidnight =
                now.Hour * 3600 +
                now.Minute * 60 +
                now.Second;

            return Content($"{unixTime},{secondsSinceMidnight}", "text/plain");
        }

        [HttpGet]
        public IActionResult Get()
        {
            var now = DateTimeOffset.Now;

            return Ok(new
            {
                unixTime = now.ToUnixTimeSeconds(),
                readAt = now.ToString("HH:mm:ss")
            });
        }
    }
}