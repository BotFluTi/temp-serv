namespace irrigation_system.Models
{
    public class TemperatureReading
    {
        public int Id { get; set; }

        public double Temperature { get; set; }

        public DateTime ReadAt { get; set; }
    }
}