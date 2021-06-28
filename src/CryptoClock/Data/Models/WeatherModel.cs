using System;

namespace CryptoClock.Data.Models
{
    public class WeatherModel
    {
        public string Units { get; init; }

        public string Location { get; init; }

        public WeatherData Current { get; set; }

        public WeatherData[] Forecast { get; set; }
    }

    public class WeatherData
    {
        public DateTime Timestamp { get; set; }

        public int TemperatureLow { get; init; }

        public int TemperatureHigh { get; init; }

        public string Image { get; init; }
    }
}
