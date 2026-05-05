using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using irrigation_system.Models;

namespace irrigation_system.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        public class TemperatureDto
        {
            [JsonPropertyName("temperature")]
            public string Temperature { get; set; } = string.Empty;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TemperatureDto data)
        {
            TemperatureStore.LastTemperature = data.Temperature;

            Console.WriteLine($"Received temperature: {data.Temperature}");

            return Ok(new { status = "received" });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { temperature = TemperatureStore.LastTemperature });
        }
    }
}