using Microsoft.AspNetCore.Mvc;
using irrigation_system.Models;
using System.Text;

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

            TemperatureStore.LastTemperature = raw;

            return Ok(new
            {
                status = "received",
                body = raw
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                temperature = TemperatureStore.LastTemperature
            });
        }
    }
}