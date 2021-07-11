using CryptoClock.Data.Models;
using System;
using System.Threading.Tasks;

namespace CryptoClock.Data.Dummy
{
    public class DummyBitcoinDataProvider : IDataProvider
    {
        public Task<CryptoModel> EnrichAsync(CryptoModel model)
        {
            return Task.FromResult(model with
            {
                Bitcoin = new BitcoinModel(new[]
                {
                    new BlockModel(689325, DateTime.UtcNow, 1000, 900, 5, 2, 100),
                    new BlockModel(689324, DateTime.UtcNow.AddMinutes(-10), 999, 1_000_000, 3, 1, 18),
                    new BlockModel(689323, DateTime.UtcNow.AddMinutes(-20), 1001, 3_000, 40, 1, 10),
                    new BlockModel(689322, DateTime.UtcNow.AddMinutes(-30), 101, 3_000, 40, 1, 10)
                },
                new BlockModel(0, DateTime.UtcNow.AddMinutes(10), 101, 3_000, 40, 1, 10))
            });
        }
    }
}
