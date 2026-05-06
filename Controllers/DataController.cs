using Microsoft.AspNetCore.Mvc;
using irrigation_system.Models;
using System.Text;
using System.Text.Json;

namespace irrigation_system.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string raw;

            try
            {
                using var reader = new StreamReader(
                    Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: false
                );

                raw = await reader.ReadToEndAsync();
            }
            catch (Microsoft.AspNetCore.Server.Kestrel.Core.BadHttpRequestException ex)
            {
                Console.WriteLine("BAD HTTP REQUEST:");
                Console.WriteLine(ex.Message);

                return BadRequest(new
                {
                    error = "Invalid or incomplete HTTP request body",
                    details = ex.Message
                });
            }
            catch (IOException ex)
            {
                Console.WriteLine("IO ERROR WHILE READING BODY:");
                Console.WriteLine(ex.Message);

                return BadRequest(new
                {
                    error = "Could not read request body",
                    details = ex.Message
                });
            }

            Console.WriteLine("RAW:");
            Console.WriteLine(raw);

            if (string.IsNullOrWhiteSpace(raw))
            {
                return BadRequest(new { error = "Empty body" });
            }

            string temperatureValue = raw;

            try
            {
                var dto = JsonSerializer.Deserialize<TemperatureDto>(raw);

                if (!string.IsNullOrWhiteSpace(dto?.Temperature))
                {
                    temperatureValue = dto.Temperature;
                }
            }
            catch
            {
                // Dacă nu este JSON valid, păstrăm raw-ul ca fallback.
                temperatureValue = raw;
            }

            TemperatureStore.LastTemperature = temperatureValue;
            TemperatureStore.LastReadAt = DateTime.Now;

            return Ok(new
            {
                status = "received",
                temperature = TemperatureStore.LastTemperature,
                readAt = TemperatureStore.LastReadAt
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                temperature = TemperatureStore.LastTemperature,
                readAt = TemperatureStore.LastReadAt
            });
        }
    }
}