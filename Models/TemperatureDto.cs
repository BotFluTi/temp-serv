using System.Text.Json.Serialization;

namespace irrigation_system.Models
{
    public class TemperatureDto
    {
        [JsonPropertyName("temperature")]
        public string? Temperature { get; set; }

        [JsonPropertyName("readAtUnix")]
        public long ReadAtUnix { get; set; }

        [JsonPropertyName("readAt")]
        public string? ReadAt { get; set; }
    }
}