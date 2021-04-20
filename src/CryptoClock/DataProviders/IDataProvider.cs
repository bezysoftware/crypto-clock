using System.Threading.Tasks;
using CryptoClock.Models;

namespace CryptoClock.DataProviders
{
    public interface IDataProvider
    {
        Task<CryptoModel> EnrichAsync(CryptoModel model);
    }
}
