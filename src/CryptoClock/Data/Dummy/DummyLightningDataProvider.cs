using CryptoClock.Data.Models;
using System.Threading.Tasks;

namespace CryptoClock.Data.Dummy
{
    public class DummyLightningDataProvider : IDataProvider
    {
        public Task<CryptoModel> EnrichAsync(CryptoModel model)
        {
            return Task.FromResult(model with
            {
                Lightning = new LightningModel(100, 40, 10, 20)
            });
        }
    }
}
