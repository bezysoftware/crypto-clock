using System.Collections.Generic;
using System.Linq;

namespace CryptoClock.Data.Models
{
    public record BitcoinModel(
        IEnumerable<BlockModel> LastBlocks,
        BlockModel MempoolBlock)
    {
        public IEnumerable<BlockModel> BlocksWithMempool => MempoolBlock != null
            ? LastBlocks.Prepend(MempoolBlock)
            : LastBlocks;
    }
}
