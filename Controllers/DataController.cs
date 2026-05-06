using Microsoft.AspNetCore.Mvc;
using irrigation_system.Models;
using irrigation_system.Data;
using System.Globalization;
using System.Text.Json;

namespace irrigation_system.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DataController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TemperatureDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Temperature))
            {
                return BadRequest(new { error = "Temperature is required" });
            }

            if (!double.TryParse(
                    request.Temperature,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out double temperatureValue))
            {
                return BadRequest(new
                {
                    error = "Invalid temperature value",
                    received = request.Temperature
                });
            }

            var reading = new TemperatureReading
            {
                Temperature = temperatureValue,
                ReadAt = DateTime.Now
            };

            _context.TemperatureReadings.Add(reading);
            await _context.SaveChangesAsync();

            TemperatureStore.LastTemperature = temperatureValue.ToString("0.0", CultureInfo.InvariantCulture);
            TemperatureStore.LastReadAt = reading.ReadAt;

            Console.WriteLine($"Temperature: {reading.Temperature}");
            Console.WriteLine($"ReadAt: {reading.ReadAt:dd.MM.yyyy HH:mm:ss}");

            return Ok(new
            {
                status = "received",
                temperature = reading.Temperature,
                readAt = reading.ReadAt
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lastReading = _context.TemperatureReadings
                .OrderByDescending(x => x.ReadAt)
                .FirstOrDefault();

            if (lastReading == null)
            {
                return Ok(new
                {
                    temperature = (double?)null,
                    readAt = (DateTime?)null
                });
            }

            return Ok(new
            {
                temperature = lastReading.Temperature,
                readAt = lastReading.ReadAt
            });
        }

        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var readings = _context.TemperatureReadings
                .OrderByDescending(x => x.ReadAt)
                .Take(50)
                .Select(x => new
                {
                    x.Id,
                    x.Temperature,
                    x.ReadAt
                })
                .ToList();

            return Ok(readings);
        }
    }
}