using System.Text.Json.Serialization;

namespace irrigation_system.Models
{
    public class TemperatureDto
    {
        [JsonPropertyName("temperature")]
        public string Temperature { get; set; } = string.Empty;
    }
}