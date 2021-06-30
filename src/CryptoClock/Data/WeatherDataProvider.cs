using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoClock.Configuration;
using CryptoClock.Extensions;
using CryptoClock.Data.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoClock.Data
{
    // https://openweathermap.org/api
    public class WeatherDataProvider : IDataProvider
    {
        private const string WeatherApiUrl = "https://api.weatherapi.com/v1/forecast.json?key={0}&q={1}&days={2}&aqi=no&alerts=no";
        private const int ForecastDays = 3;
        private const string CacheKey = "WeatherCacheKey";
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromHours(3);

        private readonly HttpClient http;
        private readonly IMemoryCache cache;
        private readonly IOptions<WeatherConfig> weatherOptions;

        public WeatherDataProvider(HttpClient http, IMemoryCache cache, IOptions<WeatherConfig> weatherOptions)
        {
            this.http = http;
            this.cache = cache;
            this.weatherOptions = weatherOptions;
        }

        public async Task<CryptoModel> EnrichAsync(CryptoModel model)
        {
            var c = this.weatherOptions.Value;
            
            var weather = await this.cache.GetOrCreateAsync(CacheKey, x =>
            {
                x.AbsoluteExpirationRelativeToNow = CacheExpiration;
                return this.http.GetObjectAsync<Weather>(string.Format(WeatherApiUrl, c.WeatherApiKey, c.Location, ForecastDays));
            });

            var units = c.Units switch
            {
                WeatherUnits.Imperial => "°F",
                WeatherUnits.Metric => "°C",
                _ => "?"
            };

            var today = weather.forecast.forecastday.First(x => x.date.Date == DateTime.UtcNow.Date);
            var now = today.hour.First(x => x.time.Hour >= DateTime.UtcNow.Hour);

            var result = new WeatherModel(
                units,
                c.Location,
                new WeatherData(
                    now.time,
                    GetTemperature(c.Units, now.temp_c, now.temp_f),
                    GetTemperature(c.Units, now.temp_c, now.temp_f),
                    GetImagePath(now.is_day == 1, now.condition.code)
                ),
                weather.forecast.forecastday
                    .SkipWhile(x => x.date.Date < DateTime.UtcNow.Date)
                    .Select(x => new WeatherData(
                        x.date,
                        GetTemperature(c.Units, today.day.mintemp_c, today.day.mintemp_f),
                        GetTemperature(c.Units, today.day.maxtemp_c, today.day.maxtemp_f),
                        GetImagePath(true, x.day.condition.code)))
                    .ToArray()
            );

            return model with
            {
                Weather = result
            };
        }

        private string GetImagePath(bool day, int code)
        {
            var dayNight = day ? "Day" : "Night";
            return $"Assets/Weather/{dayNight}/{code}.png";
        }

        private int GetTemperature(WeatherUnits units, float c, float f)
        {
            return units switch
            {
                WeatherUnits.Imperial => (int)f,
                WeatherUnits.Metric => (int)c,
                _ => throw new NotImplementedException()
            };
        }

        internal class Weather
        {
            public Location location { get; set; }
            public Current current { get; set; }
            public Forecast forecast { get; set; }
        }

        internal class Location
        {
            public string name { get; set; }
            public string region { get; set; }
            public string country { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
            public string tz_id { get; set; }
            public int localtime_epoch { get; set; }
            public string localtime { get; set; }
        }

        internal class Current
        {
            [JsonConverter(typeof(DateTimeConverter))]
            public DateTime last_updated { get; set; } 
            public int last_updated_epoch { get; set; }
            public float temp_c { get; set; }
            public float temp_f { get; set; }
            public int is_day { get; set; }
            public Condition condition { get; set; }
            public float wind_mph { get; set; }
            public float wind_kph { get; set; }
            public int wind_degree { get; set; }
            public string wind_dir { get; set; }
            public float pressure_mb { get; set; }
            public float pressure_in { get; set; }
            public float precip_mm { get; set; }
            public float precip_in { get; set; }
            public int humidity { get; set; }
            public int cloud { get; set; }
            public float feelslike_c { get; set; }
            public float feelslike_f { get; set; }
            public float vis_km { get; set; }
            public float vis_miles { get; set; }
            public float uv { get; set; }
            public float gust_mph { get; set; }
            public float gust_kph { get; set; }
        }

        internal class Condition
        {
            public string text { get; set; }
            public string icon { get; set; }
            public int code { get; set; }
        }

        internal class Forecast
        {
            public Forecastday[] forecastday { get; set; }
        }

        internal class Forecastday
        {
            [JsonConverter(typeof(DateConverter))]
            public DateTime date { get; set; }
            public int date_epoch { get; set; }
            public Day day { get; set; }
            public Hour[] hour { get; set; }
        }

        internal class Day
        {
            public float maxtemp_c { get; set; }
            public float maxtemp_f { get; set; }
            public float mintemp_c { get; set; }
            public float mintemp_f { get; set; }
            public float avgtemp_c { get; set; }
            public float avgtemp_f { get; set; }
            public float maxwind_mph { get; set; }
            public float maxwind_kph { get; set; }
            public float totalprecip_mm { get; set; }
            public float totalprecip_in { get; set; }
            public float avgvis_km { get; set; }
            public float avgvis_miles { get; set; }
            public float avghumidity { get; set; }
            public int daily_will_it_rain { get; set; }
            public string daily_chance_of_rain { get; set; }
            public int daily_will_it_snow { get; set; }
            public string daily_chance_of_snow { get; set; }
            public Condition condition { get; set; }
            public float uv { get; set; }
        }

        internal class Hour
        {
            [JsonConverter(typeof(DateTimeConverter))]
            public DateTime time { get; set; }
            public int time_epoch { get; set; }
            public float temp_c { get; set; }
            public float temp_f { get; set; }
            public int is_day { get; set; }
            public Condition condition { get; set; }
            public float wind_mph { get; set; }
            public float wind_kph { get; set; }
            public int wind_degree { get; set; }
            public string wind_dir { get; set; }
            public float pressure_mb { get; set; }
            public float pressure_in { get; set; }
            public float precip_mm { get; set; }
            public float precip_in { get; set; }
            public int humidity { get; set; }
            public int cloud { get; set; }
            public float feelslike_c { get; set; }
            public float feelslike_f { get; set; }
            public float windchill_c { get; set; }
            public float windchill_f { get; set; }
            public float heatindex_c { get; set; }
            public float heatindex_f { get; set; }
            public float dewpoint_c { get; set; }
            public float dewpoint_f { get; set; }
            public int will_it_rain { get; set; }
            public string chance_of_rain { get; set; }
            public int will_it_snow { get; set; }
            public string chance_of_snow { get; set; }
            public float vis_km { get; set; }
            public float vis_miles { get; set; }
            public float gust_mph { get; set; }
            public float gust_kph { get; set; }
            public float uv { get; set; }
        }
    }
}
