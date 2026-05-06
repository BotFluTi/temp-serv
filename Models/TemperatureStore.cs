namespace irrigation_system.Models
{
    public static class TemperatureStore
    {
        public static string LastTemperature { get; set; } = "No data yet";

        public static DateTime? LastReadAt { get; set; } = null;
    }
}