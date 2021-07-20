using BitcoinRpc.CoreRPC;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoClock.Data.Bitcoin
{
    internal class Mempool
    {
        private readonly Dictionary<string, ExtendedTransaction> cache;
        private readonly Blockchain blockchain;
        private readonly RawTransaction tx;

        public Mempool(Blockchain blockchain, RawTransaction tx)
        {
            this.cache = new Dictionary<string, ExtendedTransaction>();
            this.blockchain = blockchain;
            this.tx = tx;
        }

        public async Task<IEnumerable<ExtendedTransaction>> GetMempoolTransactionsAsync()
        {
            var json = await this.blockchain.GetRawMempool(BitcoinRpc.Enums.ReturnFormat.ArrayOfTransactionIds);
            var ids = JsonConvert.DeserializeObject<RpcResult<string[]>>(json);

            // remove missing
            this.cache.Keys
                .Except(ids.Result)
                .ToList()
                .ForEach(x => this.cache.Remove(x));

            // add new
            foreach (var id in ids.Result.Except(this.cache.Keys))
            {
                var txJson = await this.tx.GetRawTransaction(id, BitcoinRpc.Enums.TXReturnType.JSON);
                var mempoolJson = await this.blockchain.GetMemPoolEntry(id);
                
                var tx = JsonConvert.DeserializeObject<RpcResult<Transaction>>(txJson).Result;
                var mempool = JsonConvert.DeserializeObject<RpcResult<MempoolEntry>>(mempoolJson).Result;

                this.cache[id] = new ExtendedTransaction(mempool.Fee * 100_000_000, tx.Size, tx.VSize, tx.Weight);
            }

            return this.cache.Values;
        }
    }
}
