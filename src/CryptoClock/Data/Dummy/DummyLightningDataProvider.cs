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
                Lightning = new LightningModel(
                    new LightningWallet(1.1005M, 0.0045M),
                    new [] 
                    {
                        new LightningChannelModel("ACINQ", 1.1M, 1.005M),
                        new LightningChannelModel("LNDHub", 0.1M, 0.2M),
                    })
            });
        }
    }
}
