using System;
using System.Threading.Tasks;
using CryptoClock.Models;

namespace CryptoClock.DataProviders
{
    public class DateTimeDataProvider : IDataProvider
    {
        public Task<CryptoModel> EnrichAsync(CryptoModel model)
        {
            return Task.FromResult(model with { DateTime = DateTime.Now });
        }
    }
}
