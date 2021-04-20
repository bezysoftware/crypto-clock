using System;

namespace CryptoClock.Models
{
    public record CryptoModel
    {
        public DateTime DateTime { get; init; }

        public LightningModel Lightning { get; init; }
        
        public WeatherModel Weather { get; init; }

        public PriceModel Price { get; init; }
    }
}
