namespace HomeRun.NotificationService
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.53556);

        public string? Summary { get; set; }
    }
}