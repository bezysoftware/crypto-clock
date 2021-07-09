using CryptoClock.Data.Models;
using System;
using System.Collections.Generic;

namespace CryptoClock.Data.Bitcoin
{
    internal static class MempoolBlockCalculator
    {
        public static BlockModel CalculateMempoolBlock(IEnumerable<Transaction> txs)
        {
            return new BlockModel(0, DateTime.UtcNow, 10, 1000, 5, 1, 10);
        }
    }
}
