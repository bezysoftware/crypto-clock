using CryptoClock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoClock.Data.Bitcoin
{
    internal static class MempoolBlockCalculator
    {
        private const int MaxBlockSize = 4_000_000 * 8;

        public static BlockModel CalculateMempoolBlock(IEnumerable<ExtendedTransaction> txs)
        {
            var blockTxs = txs
                .OrderByDescending(x => x.Weight)
                .TakeWhileAggregate(0.0, (acc, tx) => acc + tx.Weight, acc => acc <= MaxBlockSize)
                .ToArray();

            if (!blockTxs.Any()) 
            {
                return null;
            }

            var size = blockTxs.Sum(x => x.Size);
            var min = blockTxs.Min(x => x.FeePerVsize);
            var max = blockTxs.Max(x => x.FeePerVsize);
            var median = blockTxs.Median(x => x.FeePerVsize);

            return new BlockModel(0, DateTime.UtcNow, blockTxs.Length, size, median, min, max);
        }
    }
}
