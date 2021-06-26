using CryptoClock.Data.Models;

namespace CryptoClock.Configuration
{
    public class WeatherConfig
    {
        public string WeatherApiKey { get; init; }

        public string Location { get; init; }
        
        public WeatherUnits Units { get; init; }
    }
}
