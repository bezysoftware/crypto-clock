using System;

namespace CryptoClock.Data.Models
{
    public record CryptoModel
    {
        public DateTime DateTime { get; init; }

        public LightningModel Lightning { get; init; }
        
        public WeatherModel Weather { get; init; }

        public PriceModel Price { get; init; }

        public BitcoinModel Bitcoin { get; init; }
    }
}
