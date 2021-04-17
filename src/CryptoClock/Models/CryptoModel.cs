using System;

namespace CryptoClock.Models
{
    public class CryptoModel
    {
        public DateTime DateTime { get; set; }

        public LightningModel Lightning { get; set; }
        
        public WeatherModel Weather { get; set; }
    }
}
