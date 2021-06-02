using System.Threading.Tasks;
using CryptoClock.Data.Models;

namespace CryptoClock.Data
{
    public interface IDataProvider
    {
        Task<CryptoModel> EnrichAsync(CryptoModel model);
    }
}
