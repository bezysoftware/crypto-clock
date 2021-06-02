using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoClock.Configuration;
using CryptoClock.Extensions;
using CryptoClock.Data.Models;
using Microsoft.Extensions.Options;

namespace CryptoClock.Data
{
    public class PriceDataProvider : IDataProvider
    {
        private const string GeckoUrl = "https://api.coingecko.com/api/v3/simple/price?ids={0}&vs_currencies={1}";
        private const string BitcoinId = "bitcoin";

        private readonly HttpClient http;
        private readonly IOptions<PriceConfig> config;

        public PriceDataProvider(HttpClient http, IOptions<PriceConfig> config)
        {
            this.http = http;
            this.config = config;
        }
        
        public async Task<CryptoModel> EnrichAsync(CryptoModel model)
        {
            var currency = this.config.Value.Currency;
            var url = string.Format(GeckoUrl, BitcoinId, currency);
            var response = await this.http.GetObjectAsync<Dictionary<string, Dictionary<string, decimal>>>(url);
            var price = response[BitcoinId][currency];

            return model with 
            {
                Price = new PriceModel(currency, price)
            };
        }
    }
}
