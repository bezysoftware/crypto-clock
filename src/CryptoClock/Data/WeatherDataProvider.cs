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

namespace CryptoClock.Data
{
    // https://openweathermap.org/api
    public class WeatherDataProvider : IDataProvider
    {
        private const string WeatherApiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units={2}";
        private const string ImageUrl = "http://openweathermap.org/img/wn/{0}@2x.png";

        private readonly HttpClient http;
        private readonly IOptions<WeatherConfig> weatherOptions;

        public WeatherDataProvider(HttpClient http, IOptions<WeatherConfig> weatherOptions)
        {
            this.http = http;
            this.weatherOptions = weatherOptions;
        }

        public async Task<CryptoModel> EnrichAsync(CryptoModel model)
        {
            var c = this.weatherOptions.Value;
            var weather = await this.http.GetObjectAsync<Weather>(string.Format(WeatherApiUrl, c.Location, c.OpenWeatherMapApiKey, c.Units));
            var icon = weather.weather.FirstOrDefault()?.icon;
            var unit = c.Units switch
            {
                WeatherUnits.Imperial => "°F",
                WeatherUnits.Metric => "°C",
                WeatherUnits.Standard => "K",
                _ => "?"
            };

            var imagePath = await GetImagePathAsync(icon);

            return model with 
            {
                Weather = new WeatherModel 
                {
                    Units = unit,
                    Location = c.Location,
                    Temperature = (int)weather.main.feels_like,
                    Image = imagePath
                }
            };
        }

        private async Task<string> GetImagePathAsync(string icon)
        {
            if (icon == null) 
            {
                return null;
            }

            var appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Consts.ImagesFolderName);
            var imagePath = Path.Combine(appdata, $"{icon}.png");

            if (File.Exists(imagePath))
            {
                return imagePath;
            }

            Directory.CreateDirectory(appdata);

            var response = await this.http.GetAsync(string.Format(ImageUrl, icon));

            using (var fs = new FileStream(imagePath, FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }

            return imagePath;
        }

        internal class WeatherIcon
        {
            public string icon { get; set; }
        }

        internal class WeatherMain
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
        }

        internal class Weather
        {
            public List<WeatherIcon> weather { get; set; }
            public WeatherMain main { get; set; }
        }
    }
}
