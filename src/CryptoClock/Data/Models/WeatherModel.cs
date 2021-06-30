using System;

namespace CryptoClock.Data.Models
{
    public record WeatherModel(
        string Units, 
        string Location,
        WeatherData Current,
        WeatherData[] Forecast)
    { }

    public record WeatherData(
        DateTime Timestamp,
        int TemperatureLow,
        int TemperatureHigh,
        string Image,
        string Description)
    { }
}
