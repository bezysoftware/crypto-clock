using CryptoClock.Models;

namespace CryptoClock.Configuration
{
    public class WeatherConfig
    {
        public string OpenWeatherMapApiKey { get; set; }

        public string Location { get; set; }
        
        public WeatherUnits Units { get; set; }
    }
}
