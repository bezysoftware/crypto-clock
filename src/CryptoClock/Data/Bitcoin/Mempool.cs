using BitcoinRpc.CoreRPC;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoClock.Data.Bitcoin
{
    internal class Mempool
    {
        private Blockchain blockchain;
        private readonly Dictionary<string, Transaction> cache;

        public Mempool(Blockchain blockchain)
        {
            this.blockchain = blockchain;
            this.cache = new Dictionary<string, Transaction>();
        }

        public async Task<IEnumerable<Transaction>> GetMempoolTransactionsAsync()
        {
            var json = await this.blockchain.GetRawMempool(BitcoinRpc.Enums.ReturnFormat.ArrayOfTransactionIds);
            var ids = JsonConvert.DeserializeObject<string[]>(json);

            // remove missing
            this.cache.Keys
                .Except(ids)
                .ToList()
                .ForEach(x => this.cache.Remove(x));

            // add new
            foreach (var id in ids.Except(this.cache.Keys))
            {
                var tx = await this.blockchain.GetMemPoolEntry(id);
                this.cache[id] = JsonConvert.DeserializeObject<Transaction>(tx);
            }

            return this.cache.Values;
        }
    }
}
