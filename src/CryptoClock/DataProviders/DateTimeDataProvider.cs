using System;
using System.Threading.Tasks;
using CryptoClock.Models;

namespace CryptoClock.DataProviders
{
    public class DateTimeDataProvider : IDataProvider
    {
        public Task ProvideForAsync(CryptoModel model)
        {
            model.DateTime = DateTime.Now;
            return Task.CompletedTask;
        }
    }
}
