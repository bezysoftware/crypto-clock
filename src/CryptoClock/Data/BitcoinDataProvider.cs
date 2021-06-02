using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using BitcoinRpc;
using BitcoinRpc.CoreRPC;
using CryptoClock.Configuration;
using CryptoClock.Data.Models;
using Microsoft.Extensions.Options;

namespace CryptoClock.Data
{
    public class BitcoinDataProvider : IDataProvider
    {
        private readonly BitcoinConfig config;
        private readonly Blockchain blockchain;

        public JsonSerializerOptions jsonOptions { get; }

        public BitcoinDataProvider(IOptions<BitcoinConfig> options)
        {
            this.config = options.Value;
            
            var client = new BitcoinClient(this.config.ServiceUrl, $"{this.config.Username}:{this.config.Password}");
            this.blockchain = new Blockchain(client);
            this.jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }
        public async Task<CryptoModel> EnrichAsync(CryptoModel model)
        {
            var result = model with
            {
                LastBlocks = await GetBlocksAsync(this.config.BlockCount)
            };

            return result;
        }

        private async Task<IEnumerable<BlockModel>> GetBlocksAsync(int count) 
        {
            var hash = await RunBitcoinQueryAsync<string>(x => x.GetBestBlockHash());
            var result = new List<BlockModel>();

            while(count-- > 0)
            {
                var block = await RunBitcoinQueryAsync<Block>(x => x.GetBlock(hash));
                var timestamp = DateTimeOffset.FromUnixTimeSeconds(block.Time).UtcDateTime;
                
                result.Add(new BlockModel(block.Height, timestamp, block.NTx));
                hash = block.PreviousBlockHash; 
            }

            return result;
        }

        private async Task<T> RunBitcoinQueryAsync<T>(Func<Blockchain, Task<string>> call)
        {
            var response = await call(this.blockchain);
            var result = JsonSerializer.Deserialize<RpcResult<T>>(response, this.jsonOptions);

            return result.Result;
        }

        internal class RpcResult<T> 
        {
            public T Result { get; set; }
        }

        internal class Block
        {
            public int Height { get; set; }
            public int Time { get; set; }
            public int NTx { get; set; }
            public string PreviousBlockHash { get; set; }
        }
    }
}
